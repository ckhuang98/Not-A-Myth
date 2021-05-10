
// Functions specifically for the player that have to be reused often
public class PlayerAudioManager : ObjectAudioManager
{
    public void playFootstepSFX(){
        PlayRandomSoundInGroup("footsteps", true);
    }
    public void playSlashSFX()
    {
        PlayRandomSoundInGroup("slashes");
    }
    public void playHurtSFX()
    {
        PlayRandomSoundInGroup("hurt");
    }

    public void playSizzleSFX()
    {
        PlayRandomSoundInGroup("sizzle");
    }


}
