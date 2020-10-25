using Onbox.GridGenerator.Abstractions;
using Onbox.GridGenerator.Models;
using System.Collections.Generic;
using System.Linq;

namespace Onbox.GridGenerator.Services
{
    /// <summary>
    ///  I Can even run this in ASP.Net
    /// </summary>
    public class GridInfoService : IGridInfoService
    {
        private readonly GridSettings gridSettings;
        private readonly IGridInfoFactory gridInfoFactory;

        public GridInfoService(
            GridSettings gridSettings,
            IGridInfoFactory gridInfoFactory
            )
        {
            this.gridSettings = gridSettings;
            this.gridInfoFactory = gridInfoFactory;
        }

        public string GetGridName(int gridIndex, bool isVertical)
        {
            string name = isVertical ? $"{gridIndex + 1}" : $"{(char)('A' + gridIndex)}";
            return name;
        }

        public double GetTotalOffset(List<GridInfo> grids)
        {
            var totalOffset = 0.0;
            foreach (var existingGrid in grids)
            {
                totalOffset += existingGrid.Offset;
            }
            return totalOffset;
        }

        public double GetGridTotalOffset(GridInfo grid, List<GridInfo> grids)
        {
            var index = grids.IndexOf(grid);

            var currentGrid = grids[index];
            if (index == 0)
            {
                return 0;
            }
            var previousGrid = grids[index - 1];
            var totalOffset = previousGrid.TotalOffset + currentGrid.Offset;

            return totalOffset;
        }

        public double GetGridLength(List<GridInfo> perpendicularGrids)
        {
            var totalOffset = GetTotalOffset(perpendicularGrids);
            if (totalOffset < this.gridSettings.MinimumLength)
            {
                totalOffset = this.gridSettings.MinimumLength;
            }

            return totalOffset;
        }

        public List<GridInfo> AddNewGrid(List<GridInfo> parallelGrids, bool isVertical)
        {
            double previousOffset = this.GetTotalOffset(parallelGrids);
            GridInfo newGrid;
            if (isVertical)
            {
                var name = this.GetGridName(parallelGrids.Count, true);
                newGrid = this.gridInfoFactory.CreateVertical(name, previousOffset);
            }
            else
            {
                string name = this.GetGridName(parallelGrids.Count, false);
                newGrid = this.gridInfoFactory.CreateHorizontal(name, previousOffset);
            }

            var updatedGrids = parallelGrids.ToList();
            updatedGrids.Add(newGrid);

            return updatedGrids;
        }

        public List<GridInfo> RemoveGrid(List<GridInfo> grids, GridInfo gridToRemove, bool isVertical)
        {
            grids.Remove(gridToRemove);
            var newGrids = new List<GridInfo>(grids);

            return newGrids;
        }

        public List<GridInfo> UpdateGrids(List<GridInfo> parallelGrids, List<GridInfo> perpendicularGrids, bool isVertical)
        {
            var updatedGrids = new List<GridInfo>();
            for (int i = 0; i < parallelGrids.Count; i++)
            {
                var grid = parallelGrids[i];

                // Update Name
                string name = GetGridName(i, isVertical);
                grid.Name = name;

                // Update TotalOffset
                double totalOffset = GetGridTotalOffset(grid, parallelGrids);
                grid.TotalOffset = totalOffset;

                // Update length
                double length = GetGridLength(perpendicularGrids);
                grid.Length = length;

                updatedGrids.Add(grid);
            }

            return updatedGrids;
        }


    }
}
