using Capstone.Classes;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {

            VendingMachine VM = new VendingMachine("VendingMachine.txt");
            VendingMachineCLI CLI = new VendingMachineCLI(VM);
            CLI.Run();
        }
    }
}

