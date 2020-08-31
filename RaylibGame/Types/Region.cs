using System.Collections.Generic;
using System.Numerics;

namespace RaylibGame.Types {
    public struct Region {
        public List<Vector2> RegionLocations;
        public RegionType RegionType;
        public Vector2 RegionCentre;

        public Region(List<Vector2> regionLocations, RegionType regionType, Vector2 regionCentre) {
            RegionLocations = regionLocations;
            RegionType = regionType;
            RegionCentre = regionCentre;
        }
    }
}