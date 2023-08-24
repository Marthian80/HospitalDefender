using UnityEngine;

public class AudioPlayer : Singleton<AudioPlayer>
{
    [Header("BuildTower")]
    [SerializeField] private AudioClip buildTowerClip;
    [SerializeField][Range(0f, 1f)] private float buildTowerVolume = 1.0f;

    [Header("Button")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField][Range(0f, 1f)] private float buttonClickVolume = 1.0f;

    [Header("Infected")]
    [SerializeField] private AudioClip patientInfectedClip;
    [SerializeField][Range(0f, 1f)] private float patientInfectedVolume = 1.0f;

    [Header("ShootSoap")]
    [SerializeField] private AudioClip shootSoapClip;
    [SerializeField][Range(0f, 1f)] private float shootSoapVolume = 1.0f;

    [Header("DestroyEnemy")]
    [SerializeField] private AudioClip enemyDestroyedClip;
    [SerializeField][Range(0f, 1f)] private float enemyDestroyedVolume = 1.0f;

    [Header("GameEnd")]
    [SerializeField] private AudioClip gameLostClip;
    [SerializeField][Range(0f, 1f)] private float gameLostVolume = 1.0f;
    [SerializeField] private AudioClip gameWonClip;
    [SerializeField][Range(0f, 1f)] private float gameWonVolume = 1.0f;

    [Header("Music")]   
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip levelOneMusicClip;

    private AudioSource audioSource;    

    protected override void Awake()
    {
        base.Awake();
        audioSource = FindObjectOfType<AudioSource>();        
    }

    private void Start()
    {
        //start game with menu music
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        if (menuMusicClip != null && audioSource.clip != menuMusicClip)
        {
            audioSource.Stop();
            audioSource.clip = menuMusicClip;
            audioSource.loop = true;            
            audioSource.Play();
        }
    }

    public void PlayGameMusic()
    {
        if (levelOneMusicClip != null && audioSource.clip != levelOneMusicClip)
        {
            audioSource.Stop();
            audioSource.clip = levelOneMusicClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayBuildTowerClip()
    {
        if (buildTowerClip != null)
        {
            AudioSource.PlayClipAtPoint(buildTowerClip, Camera.main.transform.position, buildTowerVolume);
        }
    }

    public void PlayButtonClickClip()
    {
        if (buttonClickClip != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickClip, Camera.main.transform.position, buttonClickVolume);
        }
    }

    public void PlayPatientInfectedClip()
    {
        if (patientInfectedClip != null)
        {
            AudioSource.PlayClipAtPoint(patientInfectedClip, Camera.main.transform.position, patientInfectedVolume);
        }
    }

    public void PlayEnemyDeadClip()
    {
        if (enemyDestroyedClip != null)
        {
            AudioSource.PlayClipAtPoint(enemyDestroyedClip, Camera.main.transform.position, enemyDestroyedVolume);
        }
    }

    public void PlayShootSoapClip()
    {
        if (shootSoapClip != null)
        {
            AudioSource.PlayClipAtPoint(shootSoapClip, Camera.main.transform.position, shootSoapVolume);
        }
    }

    public void PlayGameEndClip(bool gameLost)
    {
        if (gameLost && gameLostClip != null)
        {
            AudioSource.PlayClipAtPoint(gameLostClip, Camera.main.transform.position, gameLostVolume);
        }
        else if (gameWonClip != null)
        {
            AudioSource.PlayClipAtPoint(gameWonClip, Camera.main.transform.position, gameWonVolume);
        }
    }
}