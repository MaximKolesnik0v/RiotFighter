using Interfaces;
using System.Collections.Generic;

namespace Controllers.Enemy
{
	public class EnemiesController : BaseController
	{
		private List<IView> _enemyViews;
		private List<IController> _enemyControllers;

		public EnemiesController(List<IView> enemies)
		{
			_enemyViews = enemies;
			_enemyControllers = new List<IController>();

			Init();
		}

		public override void Update(float time)
        {
            foreach (var enemyController in _enemyControllers)
            {
				enemyController.Update(time);
            }
        }

		public override void FixedUpdate(float time)
		{
			foreach (var enemyController in _enemyControllers)
			{
				enemyController.FixedUpdate(time);
			}
		}

		private void Init()
		{
			foreach (var enemy in _enemyViews)
			{
				_enemyControllers.Add(enemy.Controller);
			}
		}
	}
}