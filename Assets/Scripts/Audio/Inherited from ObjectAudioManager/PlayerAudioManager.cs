
// Functions specifically for the player that have to be reused often
public class PlayerAudioManager : ObjectAudioManager
{
    public void playFootstepSFX(){
        PlayRandomPlayerSoundInGroup("footsteps");
    }
    public void playSlashSFX()
    {
        PlayRandomPlayerSoundInGroup("slashes");
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
