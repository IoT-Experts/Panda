using UnityEngine;
using System.Collections.Generic;
using System;
public class ManagerDelegate : MonoBehaviour
{
    public static string IdFacebook;
    public static string UsernameFacebook;
    public static bool showDataQuery;

    public static string parameterAzure;
    public static long scoreAzure;

    public static List<pandafruitfarm> lstDataAzure;
    public static Action ShowData = delegate { };
    public static Action QueryIdFb = delegate { };
    public static Action LoginFacebook = delegate { };
    public static Action ShareFB = delegate { };

    public ManagerDelegate()
    {
        LoginFacebook = delegate { };
        ShowData = delegate { };
        QueryIdFb = delegate { };
        ShareFB = delegate { };
    }
}
