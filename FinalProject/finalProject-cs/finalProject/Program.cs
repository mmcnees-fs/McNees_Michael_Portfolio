﻿using System;
using System.Collections.Generic;
using System.IO;

namespace finalProject
{
    class Program
    {
        private static string folder = @"../../Files/";
        private List<Animal> pets = new List<Animal>();

        static void Main(string[] args)
        {
            Animal activePet = null;
            // I wasn't sure how to accomplish this in JS, but did stick to using instance for non-static methods here
            Program instance = new Program();

            // Load the saved animals from the stored files
            instance.LoadAnimalData();

            bool running = true;
            while (running)
            {
                instance.Header();
                Console.WriteLine(
                    "  -----------------------\n" +
                    "  ----   Main Menu   ----\n" +
                    "  -----------------------\n");
                if (activePet != null)
                    Console.WriteLine($"   Pet: {activePet.Name}\n");
                Console.WriteLine(
                    "    1. Pet Selection\n" +
                    "    2. Feeding\n" +
                    "    3. Training\n" +
                    "    4. Habitat Maintenance\n" +
                    "    0. Exit / Quit\n");

                string input = Validation.StringNotEmpty("  Select an option: ").ToLower();

                switch (input)
                {
                    case "1":
                    case "pet selection":
                        {
                            instance.Header();
                            Console.WriteLine(
                                "  -----------------------\n" +
                                "  -    Pet Selection    -\n" +
                                "  -----------------------\n");
                            activePet = instance.selectAnimal(activePet);

                            instance.Wait();
                        }
                        break;
                    case "2":
                    case "feeding":
                        {
                            instance.Header();
                            Console.WriteLine(
                                "  -----------------------\n" +
                                "  ----    Feeding    ----\n" +
                                "  -----------------------\n");
                            while (activePet == null)
                            {
                                activePet = instance.selectAnimal(activePet);
                            }

                            activePet.Eat();

                            instance.Wait();
                        }
                        break;
                    case "3":
                    case "training":
                        {
                            instance.Header();
                            Console.WriteLine(
                                "  -----------------------\n" +
                                "  ----   Training    ----\n" +
                                "  -----------------------\n");
                            while (activePet == null)
                            {
                                activePet = instance.selectAnimal(activePet);
                            }

                            instance.TrainPet(activePet);

                            instance.Wait();
                        }
                        break;
                    case "4":
                    case "habitat maintenance":
                        {
                            instance.Header();
                            Console.WriteLine(
                                "  -----------------------\n" +
                                "  - Habitat Maintenance -\n" +
                                "  -----------------------\n");
                            while (activePet == null)
                            {
                                activePet = instance.selectAnimal(activePet);
                            }

                            instance.Habitat(activePet);

                            instance.Wait();
                        }
                        break;
                    case "0":
                    case "quit":
                    case "exit":
                    case "q":
                    case "x":
                        {
                            running = false;
                        }
                        break;
                    default:
                        {
                            Console.WriteLine($"  Invalid option: {input}");
                            instance.Wait();
                        }
                        break;
                }
            }
        }

        private Animal selectAnimal(Animal currentPet)
        {
            if (pets.Count == 0)
            {
                currentPet = NewPetWizard();

                // Automatically drop the user into the new pet wizard if there isn't an active pet.
                // This makes first time running more user friendly
                if (currentPet.Name != null)
                {
                    pets.Add(currentPet);
                    Console.WriteLine($"\n  Successfully created {currentPet.Name}'s record!");
                }
                else
                {
                    Console.WriteLine("\n  An error occurred.");
                }
            }
            // Second run and beyond go here
            else
            {
                Console.WriteLine("   0. Add New Pet\n");
                for (int i = 0; i < pets.Count; i++)
                {
                    Console.WriteLine($"   {i + 1}. {pets[i].Name} the {pets[i].Species}");
                }
                int input = Validation.GetInt($"\n  Choose an option (0-{pets.Count}): ", 0, pets.Count);
                // If they pick anything other than 0, select that number from the pets list
                if (input != 0)
                {
                    currentPet = pets[input - 1];
                    Console.WriteLine($"\n  Successfully picked {currentPet.Name}!");
                }
                else
                {
                    // If they pick 0, run the new pet wizard
                    currentPet = NewPetWizard();

                    // Check to make sure the wizard worked. This should always be true, but you never know...
                    if (currentPet.Name != null)
                    {
                        pets.Add(currentPet);
                        Console.WriteLine($"\n  Successfully created {currentPet.Name}'s record!");
                    }
                    // Not sure what could cause it to get here, so a generic error it is!
                    else
                    {
                        Console.WriteLine("An error occurred.");
                    }
                }
            }
            return currentPet;
        }

        private Animal NewPetWizard()
        {
            Console.WriteLine("\n  --  New Pet Wizard  --\n");
            string petName = Validation.StringNotEmpty("   Enter the pet's name: ");
            string petGender = Validation.StringNotEmpty($"   What is {petName}'s gender (m/f): ");
            int petAge = Validation.GetInt($"   What is {petName}'s age in years: ");

            string petSpecies = Validation.StringNotEmpty($"   What is {petName}'s species: ");

            Console.WriteLine("\n  - Type Selection -\n");
            Console.WriteLine(
                "   1. Dog\n" +
                "   2. Cat\n" +
                "   3. Reptile\n" +
                "   4. Fish\n" +
                "   5. Amphibian\n" +
                "   6. Small Mammal\n" +
                "   7. Bird\n");
            // Forgot to add tolower for a bit. Could have been confusing
            string petType = Validation.StringNotEmpty($"   Choose {petName}'s type: ").ToLower();
            switch (petType)
            {
                case "1":
                case "dog":
                    {
                        // For all animal types, create a new object of that type, then save it to
                        // the local file, and return it to the method that called the wizard
                        Dog tmpDog = new Dog(petName, petGender, petAge, petSpecies);
                        SaveAnimalData(tmpDog, "dog");
                        return tmpDog;
                    }
                case "2":
                case "cat":
                    {
                        Cat tmpCat = new Cat(petName, petGender, petAge, petSpecies);
                        SaveAnimalData(tmpCat, "cat");
                        return tmpCat;
                    }
                case "3":
                case "reptile":
                    {
                        Reptile tmpReptile = new Reptile(petName, petGender, petAge, petSpecies);
                        SaveAnimalData(tmpReptile, "reptile");
                        return tmpReptile;
                    }
                case "4":
                case "fish":
                    {
                        Fish tmpFish = new Fish(petName, petGender, petAge, petSpecies);
                        SaveAnimalData(tmpFish, "fish");
                        return tmpFish;
                    }
                case "5":
                case "amphibian":
                    {
                        Amphibian tmpAmphibian = new Amphibian(petName, petGender, petAge, petSpecies);
                        SaveAnimalData(tmpAmphibian, "amphibian");
                        return tmpAmphibian;
                    }
                case "6":
                case "small mammal":
                    {
                        SmallMammal tmpMammal = new SmallMammal(petName, petGender, petAge, petSpecies);
                        SaveAnimalData(tmpMammal, "small_mammal");
                        return tmpMammal;
                    }
                case "7":
                case "bird":
                    {
                        Bird tmpBird = new Bird(petName, petGender, petAge, petSpecies);
                        SaveAnimalData(tmpBird, "bird");
                        return tmpBird;
                    }
                default:
                    {
                        Console.WriteLine($"  Invalid entry: {petSpecies}.\n  Please try again.");
                    }
                    break;
            }
            // Including this backup in case the user tricks the app into getting past the animal type switch,
            // though it shouldn't be possible
            Animal tmpAnimal = new Animal(petName, petGender, petAge, petSpecies);
            SaveAnimalData(tmpAnimal, "animal");
            return tmpAnimal;
        }

        private void TrainPet(Animal currentAnimal)
        {
            // Check that the animal is a trainable type
            // Only dogs are trainable in this app, but
            // others could be easily enabled for this feature
            while (!(currentAnimal is ITrainable))
            {
                // Make the user pick an animal that is trainable before moving on.
                // THhs seemed better than pushing you back to the main menu. With
                // more time, I would have added the 3 time loop from the JS version
                Console.WriteLine($"\n  A {currentAnimal.Species} is not a trainable species.\n\n  Please select another animal to see its commands.\n");
                currentAnimal = null;
                currentAnimal = selectAnimal(currentAnimal);
            }

            Console.WriteLine("\n  --  Training  --\n");

            Console.WriteLine($"" +
                $"   1. [train]  Teach {currentAnimal.Name} a new behavior.\n" +
                $"   2. [list]   List {currentAnimal.Name}'s known behaviors..");

            string choice = Validation.StringNotEmpty("\n  Choose an option: ").ToLower();
            switch (choice)
            {
                case "1":
                case "train":
                    {
                        string behavior = Validation.StringNotEmpty($"\n  Enter the behavior you are training {currentAnimal.Name} to do: ");
                        string signal = Validation.StringNotEmpty($"  Enter the signal you will use for {behavior}: ");

                        Console.WriteLine(((ITrainable)currentAnimal).Train(signal, behavior));
                    }
                    break;
                case "2":
                case "list":
                    {
                        // The point of the app is to provide a reference of known commands to a pet owner/zookeeper
                        // rather than a "tamagotchi" type app, so the commands just list out.
                        ((ITrainable)currentAnimal).ListCommands();
                    }
                    break;
                default:
                    {
                        Console.WriteLine($"  {choice} is not a valid option.");
                    }
                    break;
            }
            

        }

        // In hindsight, this method didn't really need to exist...
        private void Habitat(Animal currentPet)
        {
            currentPet.ListHabitatTasks();
        }

        // Save the animal data to the local file strucutre. This puts the animals in
        // their own specific files by type.
        public void SaveAnimalData(Animal currentAnimal, string type)
        {
            string filePath = folder + "Animals/" + type + "_list.csv";
            // Check if the file exists and create it if not
            // The folder strucutre DOES need to exist first
            if (!File.Exists(filePath))
            {
                FileStream newAnimalList = File.Create(filePath);
                newAnimalList.Close();
            }
            using (StreamWriter outStream = new StreamWriter(filePath, true))
            {
                outStream.WriteLine(currentAnimal.Name + "," + currentAnimal.Gender + "," + currentAnimal.Age + "," + currentAnimal.Species);
            }
        }

        // Load the data from the individual animal files into the program
        public void LoadAnimalData()
        {
            // An array of animal types to reference for loading the individual animal files
            string[] type = { "dog", "cat", "reptile", "fish", "amphibian", "small_mammal", "bird" };
            foreach (String animal in type)
            {
                string path = folder + "/Animals/" + animal + "_list.csv";
                if (File.Exists(path))
                {
                    using (StreamReader inStream = new StreamReader(path))
                    {
                        int i = 0;
                        while (inStream.Peek() > -1)
                        {
                            string line = inStream.ReadLine();
                            string[] values = line.Split(',');
                            int tmpAge = Int32.Parse(values[2]);
                            // Only create a single animal based on each entry
                            // JS identifies the animals by a separate _type property,
                            // but separate files seemed easier to manage here at the time.
                            switch (animal)
                            {
                                case "dog":
                                    {
                                        Console.WriteLine("  Loading dogs list entry...");
                                        Dog tmpDog = new Dog(values[0], values[1], tmpAge, values[3]);
                                        pets.Add(tmpDog);
                                        ++i;
                                    }
                                    break;
                                case "cat":
                                    {
                                        Console.WriteLine("  Loading cats list entry...");
                                        Cat tmpCat = new Cat(values[0], values[1], tmpAge, values[3]);
                                        pets.Add(tmpCat);
                                        ++i;
                                    }
                                    break;
                                case "reptile":
                                    {
                                        Console.WriteLine("  Loading reptiles list entry...");
                                        Reptile tmpReptile = new Reptile(values[0], values[1], tmpAge, values[3]);
                                        pets.Add(tmpReptile);
                                        ++i;
                                    }
                                    break;
                                case "fish":
                                    {
                                        Console.WriteLine("  Loading fish list entry...");
                                        Fish tmpFish = new Fish(values[0], values[1], tmpAge, values[3]);
                                        pets.Add(tmpFish);
                                        ++i;
                                    }
                                    break;
                                case "amphibian":
                                    {
                                        Console.WriteLine("  Loading amphibians list entry...");
                                        Amphibian tmpAmphibian = new Amphibian(values[0], values[1], tmpAge, values[3]);
                                        pets.Add(tmpAmphibian);
                                        ++i;
                                    }
                                    break;
                                case "small_mammal":
                                    {
                                        Console.WriteLine("  Loading small mammals list entry...");
                                        SmallMammal tmpMammal = new SmallMammal(values[0], values[1], tmpAge, values[3]);
                                        pets.Add(tmpMammal);
                                        ++i;
                                    }
                                    break;
                                case "bird":
                                    {
                                        Console.WriteLine("  Loading birds list entry...");
                                        Bird tmpBird = new Bird(values[0], values[1], tmpAge, values[3]);
                                        pets.Add(tmpBird);
                                        ++i;
                                    }
                                    break;
                                default:
                                    {
                                        Console.WriteLine($"Error reading file: {animal}_list.csv");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\n  Done!");
        }

        private void Wait()
        {
            Console.Write("\n  Press any key to continue: ");
            Console.ReadKey(true);
        }

        private void Header()
        {
            // Managed to work this into both app versions
            //  °º¤ø,¸¸,ø¤º°`°º¤ø,¸,ø¤°º¤ø,¸¸,ø¤º°`°º¤ø,¸
            Console.Clear();
            Console.WriteLine(
                @"  ____      _   _       
 |  _ \ ___| |_| |_   _ 
 | |_) / _ \ __| | | | |
 |  __/  __/ |_| | |_| |
 |_|   \___|\__|_|\__, |
                  |___/ ");
        }
    }
}
