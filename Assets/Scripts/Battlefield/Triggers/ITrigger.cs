namespace System.Collections.Generic
{
    public interface ITrigger
    {
        //Just nu respekterar den inte officiell ordning, vill man att triggers ska hända i en viss ordning
        // så måste man lägga till prioriteter på subscribers på eventbus
        public void Enable();
        public void Disable();
    }
}