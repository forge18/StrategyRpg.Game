using Godot;
using Infrastructure.HubMediator;

namespace Infrastructure.Pathfinding
{
    public class PathfindingService : IPathfindingService
    {
        private static AStar2D _pathfinder;
        private readonly IMediator _mediator;

        public PathfindingService(IMediator mediator)
        {
            _pathfinder = new AStar2D();
            _mediator = mediator;
        }

        public void AddCell(int id, Vector2 position)
        {
            _pathfinder.AddPoint(id, position);
        }

        public void ConnectCells(int cell1, int cell2)
        {
            _pathfinder.ConnectPoints(cell1, cell2);
        }

        public int GetCellIdByPosition(Vector2 position)
        {
            var result = _mediator.RunQuery(QueryTypeEnum.GetCellIdByPosition, new GetCellIdByPositionQuery(position));
            var cellId = result.ConvertResultValue<int>();

            return cellId;
        }

        public Vector2[] GetPath(Vector2 start, Vector2 end)
        {
            var startId = GetCellIdByPosition(start);
            var endId = GetCellIdByPosition(end);

            var path = _pathfinder.GetIdPath(startId, endId);

            var pathAsVector2 = new Vector2[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                var cellId = path[i];
                var cellPosition = _pathfinder.GetPointPosition(cellId);
                pathAsVector2[i] = cellPosition;
            }

            return pathAsVector2;
        }

        public void EnableCell(int id)
        {
            _pathfinder.SetPointDisabled(id, false);
        }

        public void DisableCell(int id)
        {
            _pathfinder.SetPointDisabled(id, true);
        }
    }
}