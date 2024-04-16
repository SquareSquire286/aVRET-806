using UnityEngine.Video;

/* Singleton class used to keep track of which clip to use as the 360-degree backdrop when the main therapy environment is loaded.
 * Also helps to ensure that the tutorial audio in MemoryOrbs and MainTherapy only plays the first time that the scenes are loaded.
 */

public class SceneDataSingleton
{
    static VideoClip videoClip;
    public static bool orbSceneLoadedYet, mainTherapyLoadedYet;

    public static void SetClip(VideoClip newClip)
    {
        videoClip = newClip;
    }

    public static VideoClip GetClip()
    {
        return videoClip;
    }
}
