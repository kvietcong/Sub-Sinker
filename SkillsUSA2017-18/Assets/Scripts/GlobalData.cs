public static class GlobalData {
    private static bool scrollInverse, controllerEnabled;

    public static bool ScrollInverse
    {
        get { return scrollInverse;  }
        set { scrollInverse = value; }
    }

    public static bool ControllerEnabled
    {
        get { return controllerEnabled; }
        set { controllerEnabled = value; }
    }

}
