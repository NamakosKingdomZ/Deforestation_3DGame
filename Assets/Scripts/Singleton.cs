using UnityEngine;
namespace Deforestation
{

	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
					if (_instance == null)
					{
						GameObject obj = new GameObject();
						obj.name = typeof(T).Name;
						_instance = obj.AddComponent<T>();
					}
				}
				return _instance;
			}
		}

		public static bool TryGetInstance(out T instance)
		{
			if (_instance == null)
				_instance = FindObjectOfType<T>();

			instance = _instance;
			return instance != null;
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

}
