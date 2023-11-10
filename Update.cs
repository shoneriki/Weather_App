using System;

namespace Weather_App
{
    public class Update
    {
        private readonly Dictionary<string, string> _states = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"AL", "Alabama"}, {"AK", "Alaska"}, {"AZ", "Arizona"},
            {"AR", "Arkansas"}, {"CA", "California"}, {"CO", "Colorado"},
            {"CT", "Connecticut"}, {"DE", "Delaware"}, {"FL", "Florida"},
            {"GA", "Georgia"}, {"HI", "Hawaii"}, {"ID", "Idaho"},
            {"IL", "Illinois"}, {"IN", "Indiana"}, {"IA", "Iowa"},
            {"KS", "Kansas"}, {"KY", "Kentucky"}, {"LA", "Louisiana"},
            {"ME", "Maine"}, {"MD", "Maryland"}, {"MA", "Massachusetts"},
            {"MI", "Michigan"}, {"MN", "Minnesota"}, {"MS", "Mississippi"},
            {"MO", "Missouri"}, {"MT", "Montana"}, {"NE", "Nebraska"},
            {"NV", "Nevada"}, {"NH", "New Hampshire"}, {"NJ", "New Jersey"},
            {"NM", "New Mexico"}, {"NY", "New York"}, {"NC", "North Carolina"},
            {"ND", "North Dakota"}, {"OH", "Ohio"}, {"OK", "Oklahoma"},
            {"OR", "Oregon"}, {"PA", "Pennsylvania"}, {"RI", "Rhode Island"},
            {"SC", "South Carolina"}, {"SD", "South Dakota"}, {"TN", "Tennessee"},
            {"TX", "Texas"}, {"UT", "Utah"}, {"VT", "Vermont"},
            {"VA", "Virginia"}, {"WA", "Washington"}, {"WV", "West Virginia"},
            {"WI", "Wisconsin"}, {"WY", "Wyoming"}
        };
        public void Run()
        {
            string currentState = "Ohio"; // You need to replace this with the actual value or a method to get the current state.

            Console.WriteLine($"You are in the state of {currentState}, Would you like to update what state you are in? [y/n]");
            string response = Console.ReadLine().Trim().ToLower();

            while (response != "y" && response != "n")
            {
                Console.WriteLine("I'm sorry, please enter 'y' or 'n'.");
                response = Console.ReadLine().Trim().ToLower();
            }

            if (response == "y")
            {
                // The user wants to update their state
                bool validState = false;
                string fullStateName; // Variable to hold the full state name from IsValidState
                while (!validState)
                {
                    Console.WriteLine("Enter the new state you are in:");
                    string newState = Console.ReadLine().Trim();

                    // Corrected call to IsValidState with the out parameter
                    if (IsValidState(newState, out fullStateName))
                    {
                        currentState = fullStateName; // Update the current state with the full name
                        validState = true; // End the loop since the state is valid
                        Console.WriteLine($"State updated to {currentState}.");
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid US state. Please enter a valid US state.");
                    }
                }
            }
            else
            {
                // The user does not want to update their state
                // Call the method that shows the information of the API
                // For example: DisplayWeatherInformationForState(currentState);
                Console.WriteLine("Displaying weather information for the current state...");
            }
        }

        private bool IsValidState(string input, out string fullStateName)
        {
            // Normalize the input to uppercase for comparison since the dictionary keys are uppercase.
            string normalizedInput = input.ToUpper();

            // Try to get the full state name using the state initial.
            if (_states.TryGetValue(normalizedInput, out fullStateName))
            {
                return true;
            }

            // If the initial wasn't found, try to find a full state name that matches the input.
            // This loop is not the most efficient for large datasets, but it's fine for the small number of U.S. states.
            foreach (var state in _states)
            {
                if (string.Equals(state.Value, input, StringComparison.OrdinalIgnoreCase))
                {
                    fullStateName = state.Value;
                    return true;
                }
            }

            fullStateName = null;
            return false;
        }
    }
}
