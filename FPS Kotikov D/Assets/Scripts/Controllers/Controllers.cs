using FPS_Kotikov_D.Controller;


namespace FPS_Kotikov_D
{
    public sealed class Controllers : IInitialization
    {
        private readonly IExecute[] _executeControllers;


        #region Properties

        public int Length => _executeControllers.Length;

        public IExecute this[int index] => _executeControllers[index];

        public Controllers()
        {

            //Группа для инициализации при старте программы
            ServiceLocator.SetService(new Inventory());
            ServiceLocator.SetService(new FlashlightController());
            ServiceLocator.SetService(new WeaponController());
            ServiceLocator.SetService(new InputController());
            ServiceLocator.SetService(new BotController());
            ServiceLocator.SetService(new PlayerController());

            // Группа Update
            _executeControllers = new IExecute[5]; 
            _executeControllers[0] = ServiceLocator.Resolve<FlashlightController>();
            _executeControllers[1] = ServiceLocator.Resolve<WeaponController>();
            _executeControllers[2] = ServiceLocator.Resolve<InputController>();
            _executeControllers[3] = ServiceLocator.Resolve<BotController>();
            _executeControllers[4] = ServiceLocator.Resolve<PlayerController>();
        }

        #endregion


        public void Initialization()
        {
            ServiceLocator.Resolve<Inventory>().Initialization();
            ServiceLocator.Resolve<WeaponController>().Initialization();

            foreach (var controller in _executeControllers)
            {
                if (controller is IInitialization initialization)
                {
                    initialization.Initialization();
                }
            }

            ServiceLocator.Resolve<InputController>().On();
            ServiceLocator.Resolve<BotController>().On();
        }

    }
}
