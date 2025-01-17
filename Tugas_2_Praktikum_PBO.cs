using System;

using System.Collections.Generic;

public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }
    public bool Stunned { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
        Stunned = false;
    }

    public abstract void Serang(Robot target);
    public abstract void Diserang(Robot penyerang);
    public abstract void GunakanKemampuan(Kemampuan kemampuan, Robot target);

    public abstract void CetakInformasi();
}

public class BosRobot : Robot
{
    public BosRobot(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan)
    {
        
    }

    public override void Serang(Robot target)
    {
        if (!Stunned)
        {
            Console.WriteLine($"{Nama} menyerang {target.Nama}!");
            target.Diserang(this);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menyerang karena terkena efek stunned.");
        }
    }

    public override void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan;
        if (Armor > 0)
        {
            if (damage > Armor)
            {
                int sisaDamage = damage - Armor;
                Armor = 0;
                Energi -= sisaDamage;
            }
            else
            {
                Armor -= damage;
            }
        }

        else
        {
            Energi -= damage;
        }

        if (Energi < 0) Energi = 0;

        Console.WriteLine($"{Nama} menerima {damage} damage. Pertahanan tersisa: {Armor}, Energi tersisa: {Energi}");
    }

    public override void GunakanKemampuan(Kemampuan kemampuan, Robot target)
    {
        if (!Stunned)
        {
            kemampuan.Gunakan(this, target);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menggunakan kemampuan karena terkena efek stunned.");
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah mati!");
    }

    public override void CetakInformasi()
    {
        {
            Console.WriteLine();
            Console.WriteLine($"Nama       :{Nama}");
            Console.WriteLine($"Energi     :{Energi}");
            Console.WriteLine($"Pertahanan :{Armor}");
            Console.WriteLine($"Serangan   :{Serangan}");
            Console.WriteLine($"Stunned    :{Stunned}");
        }
    }
}

public class RobotTempur : Robot
{
    public RobotTempur(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan)
    {

    }

    public override void Serang(Robot target)
    {
        if (!Stunned)
        {
            Console.WriteLine($"{Nama} menyerang {target.Nama}!");
            target.Diserang(this);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menyerang karena terkena efek stunned.");
        }
    }

    public override void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan;
        if (Armor > 0) 
        {
            if (damage > Armor) 
            {
                int sisaDamage = damage - Armor; 
                Armor = 0; 
                Energi -= sisaDamage; 
            }
            else 
            {
                Armor -= damage; 
            }
        }

        else
        {
            Energi -= damage;
        }

        if (Energi < 0) Energi = 0;

        Console.WriteLine($"{Nama} menerima {damage} damage. Armor tersisa: {Armor}, Energi tersisa: {Energi}");
    }

    public override void GunakanKemampuan(Kemampuan kemampuan, Robot target)
    {
        if (!Stunned)
        {
            kemampuan.Gunakan(this, target);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menggunakan kemampuan karena terkena efek stunned.");
        }
    }

    public override void CetakInformasi()
    {
        {
            Console.WriteLine();
            Console.WriteLine($"Nama       :{Nama}");
            Console.WriteLine($"Energi     :{Energi}");
            Console.WriteLine($"Armor      :{Armor}");
            Console.WriteLine($"Serangan   :{Serangan}");
            Console.WriteLine($"Stunned    :{Stunned}");
        }
    }
}

public interface Kemampuan
{
    string Nama(); 
    int Cooldown { get; set; } 
    void Gunakan(Robot pengguna, Robot target = null); 
    int LastUse(); 
}

public class Perbaikan : Kemampuan
{
    public string Nama() => "Repair"; 
    public int Cooldown { get; set; } = 2; 
    private int lastUse = -2; 

    public void Gunakan(Robot pengguna, Robot target = null)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            pengguna.Energi += 30;
            if (pengguna.Energi > Program.MaxEnergi)
                pengguna.Energi = Program.MaxEnergi;
            lastUse = Program.turn;
            Console.WriteLine($"{pengguna.Nama} menggunakan Repair dan memulihkan energi.");
        }
        else
        {
            Console.WriteLine($"Repair {pengguna.Nama} sedang cooldown.");
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class SeranganListrik : Kemampuan
{
    public string Nama() => "Electric Shock"; 
    public int Cooldown { get; set; } = 2; 
    private int lastUse = -2; 

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            target.Stunned = true;
            Console.WriteLine($"{pengguna.Nama} menyerang {target.Nama} menggunakan Electric Shock!");
            Console.WriteLine($"{target.Nama} terkena stun!");
            lastUse = Program.turn;
        }
        else
        {
            Console.WriteLine($"Electric Shock {pengguna.Nama} sedang cooldown.");
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class SeranganPlasma : Kemampuan
{
    public string Nama() => "Plasma Cannon"; 
    public int Cooldown { get; set; } = 3; 
    private int lastUse = -3; 

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            target.Armor = 0; 
            Console.WriteLine($"{pengguna.Nama} menyerang {target.Nama} menggunakan Plasma Cannon!");
            Console.WriteLine($"Armor {target.Nama} telah hancur!");
            lastUse = Program.turn;
        }
        else
        {
            Console.WriteLine($"Plasma Cannon {pengguna.Nama} sedang cooldown.");
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class PertahananSuper : Kemampuan
{
    public string Nama() => "Super Shield"; 
    public int Cooldown { get; set; } = 3; 
    private int lastUse = -3; 

    public void Gunakan(Robot pengguna, Robot target = null)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            pengguna.Armor = Program.MaxArmor; 
            lastUse = Program.turn;
            Console.WriteLine($"{pengguna.Nama} menggunakan Super Shield dan meningkatkan kekuatan armor.");
        }
        else
        {
            Console.WriteLine($"Super Shield {pengguna.Nama} sedang cooldown.");
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class SeranganMematikan : Kemampuan
{
    public string Nama() => "Death Attack";
    public int Cooldown { get; set;} = 2;
    private int lastUse = -2;

    public void Gunakan(Robot pengguna, Robot target = null)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            target.Energi -= 75; 
            if (target.Energi < 0)
                target.Energi = 0;
            Console.WriteLine($"{pengguna.Nama} menyerang {target.Nama} menggunakan Death Attack!");
            Console.WriteLine($"{target.Nama} menerima 75 damage!");
            lastUse = Program.turn;
        }
        else
        {
            Console.WriteLine($"Death Attack {pengguna.Nama} sedang cooldown.");
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class Program
{
    public static int turn = 0;
    public const int MaxEnergi = 100;
    public const int MaxArmor = 100;

    static void Main(string[] args)
    {
        RobotTempur Bumblebee = new RobotTempur("Bumblebee", 100, 50, 20);
        RobotTempur Jazz = new RobotTempur("Jazz", 100, 50, 25);
        RobotTempur Mirage = new RobotTempur("Mirage", 100, 50, 30);

        BosRobot Megatron = new BosRobot("Megatron", 200, 80, 40);
        BosRobot Starscream = new BosRobot("Starscream", 200, 80, 35);

        List<Robot> robotTempur = new List<Robot> { Bumblebee, Jazz, Mirage };
        List<Robot> bosRobot = new List<Robot> { Megatron, Starscream };

        Kemampuan electricShock = new SeranganListrik();
        Kemampuan plasmaCannon = new SeranganPlasma();
        Kemampuan deathAttack = new SeranganMematikan();
        Kemampuan repair = new Perbaikan();
        Kemampuan superShield = new PertahananSuper();

        while (robotTempur.Exists(r => r.Energi > 0) && bosRobot.Exists(b => b.Energi > 0))
        {
            turn++;

            Console.WriteLine();

            Console.WriteLine($"----- Turn {turn} -----");
            Console.WriteLine("--- Informasi Robot Tempur ---");
            robotTempur.ForEach(r => r.CetakInformasi());

            Console.WriteLine();
            Console.WriteLine("--- Informasi Bos Robot ---");
            bosRobot.ForEach(b => b.CetakInformasi());

            foreach (Robot tempur in robotTempur)
            {
                if (tempur.Energi > 0)
                {
                    bool validAction = false;

                    while (!validAction)
                    {
                        Console.WriteLine();

                        Console.WriteLine($"{tempur.Nama}'s turn:");
                        Console.WriteLine("1. Normal Attack");
                        Console.WriteLine("2. Use Ability");

                        int pilihan;
                        while (!int.TryParse(Console.ReadLine(), out pilihan) || pilihan < 1 || pilihan > 2)
                        {
                            Console.WriteLine("Pilihan tidak valid. Silakan pilih 1 atau 2.");
                        }

                        if (pilihan == 1)
                        {
                            Console.WriteLine("Pilih Bos yang akan diserang: 1. Megatron, 2. Starscream");
                            int target;
                            while (!int.TryParse(Console.ReadLine(), out target) || target < 1 || target > 2)
                            {
                                Console.WriteLine("Pilihan tidak valid. Silakan pilih 1 atau 2.");
                            }
                            tempur.Serang(target == 1 ? Megatron : Starscream);
                            validAction = true;
                        }
                        else if (pilihan == 2)
                        {
                            Console.WriteLine("Pilih Ability: 1. Electric Shock, 2. Plasma Cannon, 3. Death Attack, 4. Repair, 5. Super Shield");
                            int ability;
                            while (!int.TryParse(Console.ReadLine(), out ability) || ability < 1 || ability > 5)
                            {
                                Console.WriteLine("Pilihan tidak valid. Silakan pilih 1 hingga 5.");
                            }

                            if (ability == 1 || ability == 2 || ability == 3)
                            {
                                Console.WriteLine("Pilih Bos yang akan diserang: 1. Megatron, 2. Starscream");
                                int target;
                                while (!int.TryParse(Console.ReadLine(), out target) || target < 1 || target > 2)
                                {
                                    Console.WriteLine("Pilihan tidak valid. Silakan pilih 1 atau 2.");
                                }

                                Robot bosTarget = target == 1 ? Megatron : Starscream;
                                Kemampuan selectedAbility = ability == 1 ? electricShock : ability == 2 ? plasmaCannon : deathAttack;

                                if (selectedAbility.Cooldown + selectedAbility.LastUse() <= Program.turn)
                                {
                                    tempur.GunakanKemampuan(selectedAbility, bosTarget);
                                    validAction = true;
                                }
                                else
                                {
                                    Console.WriteLine("Kemampuan sedang dalam cooldown, silakan pilih aksi lain.");
                                }
                            }
                            else if (ability == 4)
                            {
                                if (repair.Cooldown + repair.LastUse() <= Program.turn)
                                {
                                    tempur.GunakanKemampuan(repair, null);
                                    validAction = true;
                                }
                                else
                                {
                                    Console.WriteLine("Repair sedang dalam cooldown, silakan pilih aksi lain.");
                                }
                            }
                            else if (ability == 5)
                            {
                                if (superShield.Cooldown + superShield.LastUse() <= Program.turn)
                                {
                                    tempur.GunakanKemampuan(superShield, null);
                                    validAction = true;
                                }
                                else
                                {
                                    Console.WriteLine("Super Shield sedang dalam cooldown, silakan pilih aksi lain.");
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine();

            Random rnd = new Random();
            foreach (Robot bos in bosRobot)
            {
                if (bos.Energi > 0)
                {
                    Robot targetTempur = robotTempur[rnd.Next(robotTempur.Count)];

                    if (rnd.Next(2) == 0)
                    {
                        bos.Serang(targetTempur);
                    }
                    else
                    {
                        int abilityChoice = rnd.Next(3);

                        switch (abilityChoice)
                        {
                            case 0:
                                bos.GunakanKemampuan(electricShock, targetTempur);
                                break;
                            case 1:
                                bos.GunakanKemampuan(deathAttack, targetTempur);
                                break;
                            case 2:
                                bos.GunakanKemampuan(plasmaCannon, targetTempur);
                                break;
                        }
                    }
                }
            }

            if (!robotTempur.Exists(r => r.Energi > 0))
            {
                Console.WriteLine("Bos Robot Menang!");
                break;
            }
            if (!bosRobot.Exists(b => b.Energi > 0))
            {
                Console.WriteLine("Robot Tempur Menang!");
                break;
            }

            Console.WriteLine("\nNext turn? Tekan Enter untuk melanjutkan...");
            Console.ReadLine();
        }
    }
}
