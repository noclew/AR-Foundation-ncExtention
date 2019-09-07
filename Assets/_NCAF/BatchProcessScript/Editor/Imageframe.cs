using System;

namespace DefaultNamespace
{
    [Serializable]
    public class Imageframe
    {
        public string name;
        public float vectorW_x;
        public float vectorW_y;
        public float vectorW_z;
        public float vectorH_x;
        public float vectorH_y;
        public float vectorH_z;
        public int pixel_w;
        public int pixel_h;
        public float centerpoint_x;
        public float centerpoint_y;
        public float centerpoint_z;
        public string image_filepath;        
    }
}