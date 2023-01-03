namespace RQ.Physics
{
    public static class Common
    {
        public static RQ.Direction ChooseDirection(bool includeIdle = false)
        {
            //Direction direction;
            int rnd = UnityEngine.Random.Range(0, includeIdle ? 5 : 4);
            switch (rnd)
            {
                case 0:
                    //velocity = new Vector3(speed, 0, 0);
                    return RQ.Direction.Right;
                case 1:
                    //velocity = new Vector3(-speed, 0, 0);
                    return RQ.Direction.Left;
                case 2:
                    //velocity = new Vector3(0, speed, 0);
                    return RQ.Direction.Up;
                case 3:
                    //velocity = new Vector3(0, -speed, 0);
                    return RQ.Direction.Down;
                case 4:
                    return RQ.Direction.None;
                default:
                    return RQ.Direction.Up;
            }
        }
    }
}
