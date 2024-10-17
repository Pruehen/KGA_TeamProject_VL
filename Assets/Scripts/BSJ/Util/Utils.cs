using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BSJ
{
    public static class Utils
    {
        #region asyncSceneLoad
        public static IEnumerator PrepareScene(AsyncOperation ao)
        {
            //잠시 기다려줘야 한다 어웨이크에서 호출시 오류 발생
            yield return new WaitForSeconds(.1f);

            ao = SceneManager.LoadSceneAsync("01.Scenes/MainGame");
            ao.allowSceneActivation = false;
        }
        public static void LoadWhenSceneIsPrepared(ref AsyncOperation ao)
        {
            ao.allowSceneActivation = true;
        }
        #endregion
        public static IEnumerator Fade(bool isFadeIn, CanvasGroup target, GameObject gameObject)
        {
            float timer = 0.0f;

            while(timer <= 1f)
            {
                yield return null;
                timer += Time.unscaledDeltaTime;
                target.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);
            }

            if(!isFadeIn)
            {
                gameObject.SetActive(false);
            }
        }
    }






    public static class ImageUiExtender
    {
        public static void FadeIn(this Graphic image, MonoBehaviour coroutineRunner,float time, System.Action fadeEnd = null)
        {
            coroutineRunner.StartCoroutine(HierachycallyFadeInCoroutine(image, coroutineRunner, time, fadeEnd));
        }
        public static void FadeOut(this Graphic image, MonoBehaviour coroutineRunner,float time, System.Action fadeStart = null, System.Action fadeEnd = null)
        {
            coroutineRunner.StartCoroutine(HierachycallyFadeOutCoroutine(image, coroutineRunner, time, fadeStart, fadeEnd));
        }

        private static IEnumerator HierachycallyFadeInCoroutine(Graphic image, MonoBehaviour coroutineRunner, float time, System.Action fadeEnd = null)
        {
            float alpha = 0f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            image.gameObject.SetActive(true);

            for (int i = 0; i < image.transform.childCount; i++)
            {
                image.transform.GetChild(i).gameObject.SetActive(false);
            }

            while (image.color.a < 1f)
            {
                alpha += Time.fixedUnscaledDeltaTime / time;
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                yield return null;
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

            for (int i = 0; i < image.transform.childCount; i++)
            {
                image.transform.GetChild(i).GetComponent<Graphic>()?.FadeIn(coroutineRunner, time, fadeEnd);
            }
            fadeEnd?.Invoke();
            yield break;
        }

        private static IEnumerator HierachycallyFadeOutCoroutine(Graphic image, MonoBehaviour coroutineRunner, float time, System.Action fadeStart = null, System.Action fadeEnd = null)
        {
            fadeStart?.Invoke();
            float alpha = 1f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            image.gameObject.SetActive(true);

            for (int i = 0; i < image.transform.childCount; i++)
            {
                image.transform.GetChild(i).gameObject.SetActive(false);
            }
            while (image.color.a > 0f)
            {
                alpha -= Time.fixedUnscaledDeltaTime / time;
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                yield return null;
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);

            for (int i = 0; i < image.transform.childCount; i++)
            {
                image.transform.GetChild(i).GetComponent<Graphic>()?.FadeOut(coroutineRunner, time, fadeStart, fadeEnd);
            }

            fadeEnd?.Invoke();
            yield break;
        }
    }

    public static class TransformExtender
    {
        /// <summary>
        /// 모든 자식에서 컴포넌트를 찾아 리스트로 반환해줌
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static List<T> GetChildRecursive<T>(this Transform transform)
        {
            List<Transform> list = new List<Transform>();
            List<T> result = new List<T>();
            GetAllChild(transform, ref list);

            foreach (Transform child in list)
            {
                if (child != transform)
                {
                    result.Add(child.GetComponent<T>());
                }
            }    
            return result;
        }
        /// <summary>
        /// 모든 자식 게임오브젝트를 리스트에 담아줌
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="list"></param>
        private static void GetAllChild(this Transform transform, ref List<Transform> list)
        {
            list.Add(transform);
            for (int i = 0; i < transform.childCount; i++) 
            {
                GetAllChild(transform.GetChild(i),ref list);
            }
        }
    }

}

