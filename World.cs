using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game2
{
    class World
    {
// Variables, characteristics.
        public enum GState { Paused, Waiting, Select1, Select2}
        public enum MState { Waiting, Designate}
        public enum GTool { None, Chopdown}
        GState State = GState.Waiting;
        public GTool Tool = GTool.None;
        public MState mState = MState.Waiting;
        public GraphicsHandler GFX;
        // Dimensions
        public int MaxX
        {
            get;
            protected set;
        }
        public int MaxY
        {
            get;
            protected set;
        }
        public int MaxZ
        {
            get;
            protected set;
        }
        public Tile[,,] Tiles
        {
            get;
            protected set;
        }
        Tile[,,] SelectedTiles;
        List<Tile> SpcSelectedTiles;

        //Mobiles
        List<Creature> Creatures;

        // Hovered (XYZ)
        public int X
        {
            get;
            protected set;
        } = 22;
        public int Y
        {
            get;
            protected set;
        } = 22;
        public int Z
        {
            get;
            protected set;
        } = 0;

        // Pathing Graph
        public TileGraph TileGraph;

        //Actions
        Action<Tile> cbTileChanged;

        //We might remove this and replace it with a dedicated class for handling jobs
        public Job_Queue jobQueue;

        // Materials
        public List<Material> Mats;

        //Objects
        public WorldInventory WI;

// Action Controls
        // DragSelect1
        int[] SXYZ = new int[3];

        //Drag Select2
        int[] EXYZ = new int[3];

        // Get a tile at xyz
        public Tile GetTile(int x, int y, int z)
        {
            if( (x < 0 || y < 0 || z < 0 || x > MaxX || y > MaxY || z > MaxZ)) 
            {
                return null;
            }
            return Tiles[x, y, z];
        }

        // Move the cursor
        public void MoveCursor(int dx, int dy, int dz)
        {
            // X
            this.X = CheckChange(dx, this.X, this.MaxX);
            //Y
            this.Y = CheckChange(dy, this.Y, this.MaxY);
            //Z
            this.Z = CheckChange(dz, this.Z, this.MaxZ);
        }

        // Hit 'Enter'
        public void EnterCursor()
        {
            switch (this.State)
            {
                case GState.Waiting:
                    SXYZ[0] = X;
                    SXYZ[1] = Y;
                    SXYZ[2] = Z;
                    SelectedTiles = new Tile[1, 1, 1];
                    SelectedTiles[0, 0, 0] = this.GetTile(SXYZ[0], SXYZ[1], SXYZ[2]);
                    Tiles[X, Y, Z].Selected = true;
                    this.State = GState.Select1;
                    break;
                case GState.Select1:
                    // BOX SELECT FROM SELECT1
                    EXYZ[0] = X;
                    EXYZ[1] = Y;
                    EXYZ[2] = Z;
                    // CALC SIZE OF ARRAY
                    int[] d = new int[3];
                    for (int i = 0; i <= 2; i++)
                    {
                        d[i] = (SXYZ[i] - EXYZ[i]);
                    }
                    SelectedTiles = new Tile[Math.Abs(d[0]) + 1, Math.Abs(d[1]) + 1, Math.Abs(d[2]) + 1];
                    for (int i = 0; i <= Math.Abs(d[0]); i++)
                    {
                        for (int j = 0; j <= Math.Abs(d[1]);j++)
                        {
                            for (int k = 0; k <= Math.Abs(d[2]); k++)
                            {
                                int xloc;
                                int yloc;
                                int zloc;
                                //X
                                if (EXYZ[0] > SXYZ[0])
                                {
                                    xloc = SXYZ[0] + i;
                                }
                                else
                                {
                                    xloc = SXYZ[0] - i;
                                }
                                //Y
                                if (EXYZ[1] > SXYZ[1])
                                {
                                    yloc = SXYZ[1] + j;
                                }
                                else
                                {
                                    yloc = SXYZ[1] - j;
                                }
                                //Z
                                if (EXYZ[2] > SXYZ[2])
                                {
                                    zloc = SXYZ[2] + k;
                                }
                                else
                                {
                                    zloc = SXYZ[2] - k;
                                }
                                SelectedTiles[i, j, k] = this.GetTile(xloc, yloc, zloc);
                                Tiles[xloc, yloc, zloc].Selected = true;
                                //throw new Exception(Tiles[SXYZ[0] + i, SXYZ[1] + j, SXYZ[2] + k].Selected.ToString());
                            }
                        }
                    }
                    State = GState.Select2;
                    break;
                case GState.Select2:
                    // Assign jobs to the objects in the selected zone
                    switch(Tool)
                    {
                        case GTool.Chopdown:
                            SelectByStationaryObject(ObjectType.Tree);
                            break;
                    }
                    this.Deselect();
                    State = GState.Waiting;
                    break;
            }

        }

        // Hit 'a-z' to interact with menu;
        public void MenuInteration(System.Windows.Forms.KeyEventArgs args)
        {
            if(args.KeyData == System.Windows.Forms.Keys.OemQuotes)
            {
                mState = MState.Waiting;
            };
            switch (mState)
            {
                case MState.Waiting:
                    switch (args.KeyData)
                    {
                        case System.Windows.Forms.Keys.D:
                            this.mState = MState.Designate;
                            break;
                    }
                    break;
                case MState.Designate:
                    switch (args.KeyData)
                    {
                        // C for Chopdown
                        case System.Windows.Forms.Keys.C:
                            if(State == GState.Select2 || State == GState.Select1)
                            {
                                //MakeJobs, chopdown
                                //Deselect
                                SelectByStationaryObject(ObjectType.Tree);
                                //CutDownSelectedTiles();
                                MakeJobs(JobType.Harvest);
                                SpcDeselect();
                                mState = MState.Waiting;
                                State = GState.Waiting;
                            }else if(State == GState.Select1)
                            {
                                EnterCursor();
                                //MakeJobs, chopdown
                                // Deselect
                            }else
                            {
                                //Set tool to be chopper
                                this.Tool = GTool.Chopdown;
                            }
                            break;
                    }
                    break;

            }
        }

        int CheckChange(int ds, int s, int MaxS)
        {
            // S
            if (ds > 0)
            {
                if (s + ds < MaxS)
                {
                    return s += ds;
                }
                else
                {
                    return s = MaxS;
                }
            }
            else
            {
                if (s + ds > 0)
                {
                    return s += ds;
                }
                else
                {
                    return s = 0;
                }
            }
        }

        void Deselect()
        {
            foreach(Tile t in SelectedTiles)
            {
                t.Selected = false;
            }
            SelectedTiles = null;
        }
        void SpcDeselect()
        {
            {
                foreach (Tile t in SpcSelectedTiles)
                {
                    t.Selected = false;
                }
                SpcSelectedTiles = null;
            }
        }

        void SelectByStationaryObject(ObjectType ot)
        {
            SpcSelectedTiles = new List<Tile>();
            foreach (Tile t in SelectedTiles)
            {
                if (t.stationaryObject != null)
                {
                    if (t.stationaryObject.Type == ot)
                    {
                        SpcSelectedTiles.Add(t);
                    }
                }
            }
            Deselect();
            foreach(Tile t in SpcSelectedTiles)
            {
                t.Selected = true;
            }
        }

        void CutDownSelectedTiles()
        {
            foreach (Tile t in SpcSelectedTiles)
            {
                t.DestroyStationaryObject();
            }
            SpcDeselect();
        }

        enum JobType { Harvest};
        void MakeJobs(JobType jt)
        {
            switch(jt)
            {
                case JobType.Harvest:
                    foreach(Tile t in SpcSelectedTiles)
                    {
                        MakeHarvestJob(t);
                    }
                    break;
            }
        }
        
        public void Step()
        {
            if (State == GState.Paused)
                return;

            foreach(Creature c in Creatures)
            {
                c.Update();
            }
        }

        void MakeHarvestJob(Tile t)
        {
            // Is there already an existed harvest job for this tile?
            if(t.PendingJob == null)
            {
                Job j = new Game2.Job(t, (thejob) => { OnHarvestJobComplete(t); });
                jobQueue.Enqueue(j);
                //System.Diagnostics.Debug.WriteLine("Harvest job created");
                //System.Diagnostics.Debug.WriteLine("Queue size:" + jobQueue.Count.ToString());
                t.SetJob(j);
            }
        }

        void OnHarvestJobComplete(Tile t)
        {
            t.DestroyStationaryObject();
        }

        public void AddCreature(int x, int y, int z,  Creature c)
        {
            Creatures.Add(c);
            Tiles[x, y, z].Creatures.Add(c);
            c.SetTile(Tiles[x, y, z]);
        }

        public void EnduceBasicMaterials()
        {
            //Wood types.
            string[] names = new string[10] { "alder", "ash", "aspen", "birch", "cherry", "elm", "maple", "oak", "scyamore", "walnut" };
            int[] Hardness = new int[10] { 266, 594, 158, 567, 427, 374, 652, 612, 347, 455 };
            Brush[] brushes = new Brush[10] {Brushes.Maroon, Brushes.MintCream, Brushes.FloralWhite, Brushes.LightPink, Brushes.DarkOrchid, Brushes.RosyBrown, Brushes.Honeydew, Brushes.Goldenrod,
            Brushes.Brown, Brushes.SaddleBrown};
            float[] densities = new float[10] {0.4f, 0.65f, 0.42f, 0.67f, 0.63f, 0.57f, 0.65f, 0.75f, 0.5f, 0.49f };
            for(int i = 0; i< names.Length; i++)
            {
                this.Mats.Add(new Material(names[i], brushes[i], densities[i], Hardness[i], MatType.Organic_Wood));
            }
        }

        // Callbacks
            //TileChanged
        void RegisterOnTileChanged(Action<Tile> cb)
        {
            cbTileChanged += cb;
        }
        void UnregisterOnTileChanged(Action<Tile> cb)
        {
            cbTileChanged -= cb;
        }

        void OnTileChanged(Tile t)
        {
            if (cbTileChanged == null)
                return;

            cbTileChanged(t);
            OutdateTileGraph();
        }

        // Pathing
        public void OutdateTileGraph()
        {
            this.TileGraph = null;
        }

        public World(int x = 100, int y = 100, int z = 3)
        {
            this.Tiles = new Tile[x, y, z];
            this.Mats = new List<Material>();
            this.MaxX = x - 1;
            this.MaxY = y - 1;
            this.MaxZ = z - 1;
            this.WI = new WorldInventory(this);
            this.Creatures = new List<Creature>();
            this.GFX = new GraphicsHandler(this);
            this.jobQueue = new Job_Queue();
            // Build basic materials
            EnduceBasicMaterials();
            // Build map
            for (int i = 0; i <= MaxX; i++)
            {
                for (int j = 0; j <= MaxY; j++)
                {
                    for (int k = 0; k <= MaxZ; k++)
                    {
                        Tiles[i, j, k] = new Tile(this, i, j, k);
                        //Tiles[i, j, k].RegisterOnTileChangedCB(OnTileChanged);
                    }
                }
            }
            // Build tilegraph
            this.TileGraph = new TileGraph(this);
        }

    }
}
