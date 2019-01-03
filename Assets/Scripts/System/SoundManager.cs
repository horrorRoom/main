/***********************************************************/
//サウンドマネージャー
/***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private static SoundManager singleInstance = null;
    public static SoundManager GetInstance(){
        if (singleInstance == null) singleInstance = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        return singleInstance;
    }

    class Sound
    {
        /// <summary>
        /// 再生中の名前
        /// </summary>
        public string soundName;
        /// <summary>
        /// サウンド
        /// </summary>
        public AudioSource audio;
        /// <summary>
        /// ミュート
        /// </summary>
        public bool isMute = false;
    }

    /// <summary>
    /// サウンドの再生方法
    /// </summary>
    public enum SoundPlayerMode
    {
        LOOP, // ループして流す
        ONE_PLAY, // 一回流す
    }

    /// <summary>
    /// サウンドのフェード
    /// </summary>
    public enum SoundFade
    {
        NONE,       //何もしない
        FADE_IN,    //上がっていく
        FADE_OUT    //下がっていく
    }

    /// <summary>
    /// 再生中のBGM
    /// </summary>
    private List<Sound> bgmPlayList=new List<Sound> ();

    /// <summary>
    /// 再生中のSE
    /// </summary>
    private List<Sound> sePlayList = new List<Sound>();

    [Header("Object")]
    [SerializeField]
    private AudioSource soundObj;

    [Header("Resource")]
    [SerializeField]
    List<SoundResource> soundList = new List<SoundResource>();

    [System.Serializable]
    class SoundResource
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public AudioClip audioClip;
    }

    //再生中のSE番号
    private int seSelectNumber = 0;

    //最大SE作成中
    const int MaxSE = 10;

    /// <summary>
    /// BGMをセット、再生
    /// </summary>
    public void BGMPlay(string name, SoundPlayerMode soundPlayerMode)
    {
        SoundResource soundResource = GetResource(name);

        //リソースがある場合
        if (soundResource != null)
        {
            Debug.LogError("No Sound Resouce. name="+name);
            return;
        }

        //リソースがある場合
        if (bgmPlayList.Count<=0)
        {
            for (int i = 0; i < 3; i++)
            {
                var obj = Instantiate(soundObj.gameObject, singleInstance.gameObject.transform) as GameObject;
                if (i == 0)
                {
                    soundObj.clip = soundResource.audioClip;
                    soundObj.Play();
                }

                //BGMセット
                bgmPlayList.Add(new Sound()
                {
                    soundName = name,
                    audio = soundObj
                });
            }
        }
    }

    /// <summary>
    /// SEをセット、再生
    /// </summary>
    public void SEPlay(string name)
    {
        SoundResource soundResource = GetResource(name);
        
        //まだ作成されていない場合
        if (sePlayList.Count < MaxSE)
        {
            var obj = Instantiate(soundObj.gameObject, singleInstance.gameObject.transform) as GameObject;
            AudioSource audio = obj.GetComponent<AudioSource>();
            audio.clip = soundResource.audioClip;
            //SEセット
            sePlayList.Add(new Sound()
            {
                soundName = name,
                audio = audio
            });
        }
        else
        {
            sePlayList[seSelectNumber].audio.clip = soundResource.audioClip;
            sePlayList[seSelectNumber].audio.Play();
            seSelectNumber++;
            if(seSelectNumber==MaxSE) seSelectNumber = 0;
        }
    }

    /// <summary>
    /// 保存したリソースから名前が一致する物を返す
    /// </summary>
    /// <param name="name">登録name</param>
    /// <returns>リソース情報</returns>
    private SoundResource GetResource(string name)
    {
        foreach (var resource in soundList)
        {
            if (resource.name == name) return resource;
        }
        return null;
    }

    /// <summary>
    /// サウンドをフェードさせていく
    /// </summary>
    /// <returns></returns>
    IEnumerator SoundFedeOut()
    {
        yield  return null;
    }

}
