public class CopyableModule : ObjectModule {

    void Start() => baseObject.OnCopy += BaseObject_OnCopy;

    private void BaseObject_OnCopy() {
        throw new System.NotImplementedException();
    }
}
