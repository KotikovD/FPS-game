namespace FPS_Kotikov_D.Controller
{
    public sealed class PocketPCController : BaseController, IExecute, IInitialization
    {


        #region Fileds

        private PocketPC _pocketPC;

        #endregion


        #region Methods

        public void Initialization()
        {
            _pocketPC = UnityEngine.Object.FindObjectOfType<PocketPC>();
           _pocketPC.Switch(false);
        }

        public void Execute()
        {
            if (!IsActive) return;
        }

        public override void On()
        {
            if (IsActive) return;
            base.On(_pocketPC);
            _pocketPC.Switch(true);
            _pocketPC.PocketPCBoot();

        }

        public override void Off()
        {
            if (!IsActive) return;
            base.Off();
            _pocketPC.Switch(false);
        }

        #endregion


    }
}

        