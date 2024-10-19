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






    public static class Fader
    {
        struct FadeInfo
        {
            public Transform target;
            public float targetOpacity;
            public Graphic targetGraphic;
        }
        public static void FadeIn(CanvasGroup target, MonoBehaviour coroutineRunner,float time, System.Action fadeEnd = null)
        {
            coroutineRunner.StartCoroutine(FadeInCoroutine(target, coroutineRunner, time, fadeEnd));
        }
        private static IEnumerator FadeInCoroutine(CanvasGroup target, MonoBehaviour coroutineRunner,float time, System.Action fadeEnd = null)
        {
            float alpha = 0f;
            target.alpha = alpha;

            while (target.alpha < 1f)
            {
                alpha += Time.fixedUnscaledDeltaTime / time;
                target.alpha = alpha;
                yield return null;
            }
            target.alpha = 1f;
            fadeEnd?.Invoke();
        }
        public static void FadeOut(CanvasGroup target, MonoBehaviour coroutineRunner,float time, System.Action fadeEnd = null)
        {
            coroutineRunner.StartCoroutine(FadeOutCoroutine(target, coroutineRunner, time, fadeEnd));
        }
        private static IEnumerator FadeOutCoroutine(CanvasGroup target, MonoBehaviour coroutineRunner,float time, System.Action fadeEnd = null)
        {
            float alpha = 1f;
            target.alpha = alpha;

            while (target.alpha > 0f)
            {
                alpha -= Time.fixedUnscaledDeltaTime / time;
                target.alpha = alpha;
                yield return null;
            }
            target.alpha = 0f;
            fadeEnd?.Invoke();
        }
        public static void FadeIn(Transform target, MonoBehaviour coroutineRunner,float time, System.Action fadeEnd = null)
        {
            List<List<FadeInfo>> list = new List<List<FadeInfo>>();
            List<FadeInfo> temp = new List<FadeInfo>();
            int currentDepthIndex = 0;
            int currentDepthCount = 1;
            int nextDepthCount = 0;
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(target);
            temp.Add(new FadeInfo{target = target, targetOpacity = 1f});
            while(queue.Count > 0)
            {
                List<FadeInfo> currentChild = new List<FadeInfo>();
                Transform current = queue.Dequeue();
                for(int i = 0; i < current.childCount; i++)
                {
                    Transform child = current.GetChild(i);
                    if(child.gameObject.activeInHierarchy)
                    {
                        nextDepthCount++;
                        if(child.TryGetComponent(out Graphic graphic))
                        {
                            currentChild.Add(new FadeInfo{target = child, targetOpacity = graphic.color.a});
                            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0f);
                        }
                        else
                        {
                            currentChild.Add(new FadeInfo{target = child, targetOpacity = 1f});
                        }
                        queue.Enqueue(child);
                        
                    }
                }
                currentDepthIndex++;
                if(currentDepthIndex == currentDepthCount)
                {
                    list.Add(temp);
                    temp = new List<FadeInfo>();
                    currentDepthIndex = 0;
                    currentDepthCount = nextDepthCount;
                    nextDepthCount = 0;
                }
                temp.AddRange(currentChild);
            }
            if(currentDepthIndex == currentDepthCount)
            {
                list.Add(temp);
                temp = new List<FadeInfo>();
                currentDepthCount = 0;
            }
            coroutineRunner.StartCoroutine(HierachycallyFadeInCoroutine(list,0, coroutineRunner, time, fadeEnd));
        }

        public static void FadeOut(Transform target, MonoBehaviour coroutineRunner,float time, System.Action fadeStart = null, System.Action fadeEnd = null)
        {
            coroutineRunner.StartCoroutine(HierachycallyFadeOutCoroutine(target, coroutineRunner, time, fadeStart, fadeEnd));
        }

        private static IEnumerator HierachycallyFadeInCoroutine(List<List<FadeInfo>> target, int index, MonoBehaviour coroutineRunner, float time, System.Action fadeEnd = null)
        {
            if(index == -1)
            {
                yield break;
            }
            List<FadeInfo> temp = new List<FadeInfo>();
            bool isFirst = true;

            foreach (FadeInfo i in target[index])
            {
                if(i.target.TryGetComponent(out Graphic img))
                {
                    temp.Add(new FadeInfo{target = i.target, targetOpacity = 1f, targetGraphic = img});
                }
            }
            if(temp.Count == 0)
            {
                coroutineRunner.StartCoroutine(HierachycallyFadeInCoroutine(target, GetNextDepth(target, index), coroutineRunner, time, fadeEnd));
            }

            foreach (FadeInfo i in temp)
            {
                Graphic image = i.targetGraphic;
                float alpha = 0f;
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                image.gameObject.SetActive(true);
                if(isFirst)
                {
                    isFirst = false;
                    coroutineRunner.StartCoroutine(FadeInCoroutine(image, time, i.targetOpacity, () => {
                        fadeEnd?.Invoke();
                        coroutineRunner.StartCoroutine(HierachycallyFadeInCoroutine(target, GetNextDepth(target, index), coroutineRunner, time, fadeEnd));

                    }));
                }
                else
                {
                    coroutineRunner.StartCoroutine(FadeInCoroutine(image, time, i.targetOpacity, fadeEnd));
                }

            }
            yield break;
        }
        private static int GetNextDepth(List<List<FadeInfo>> target, int index)
        {
            for(int i = index + 1; i < target.Count; i++)
            {
                if(target[i].Count > 0)
                {
                    return i;
                }
            }
            return -1;
        }

        private static IEnumerator FadeInCoroutine(Graphic target, float time, float targetOpacity, System.Action fadeEnd = null)
        {
            float alpha = 0f;
            target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);

            while (target.color.a < targetOpacity)
            {
                alpha += Time.fixedUnscaledDeltaTime / time;
                target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
                yield return null;
            }
            target.color = new Color(target.color.r, target.color.g, target.color.b, targetOpacity);
            fadeEnd?.Invoke();
        }

        private static IEnumerator HierachycallyFadeOutCoroutine(Transform target, MonoBehaviour coroutineRunner, float time, System.Action fadeStart = null, System.Action fadeEnd = null)
        {
            if(target.TryGetComponent(out Graphic image))
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

                fadeEnd?.Invoke();
            }

            for (int i = 0; i < target.childCount; i++)
            {
                FadeOut(target.GetChild(i), coroutineRunner, time, fadeStart, fadeEnd);
            }
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

