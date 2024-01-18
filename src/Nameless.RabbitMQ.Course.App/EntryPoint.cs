namespace Nameless.RabbitMQ.Course.App {
    public sealed class EntryPoint {
        #region Public Static Methods

        public static void Main(string[] args) {
            using var runner = new CourseRunner([
                typeof(EntryPoint).Assembly,
                typeof(ExerciseBase).Assembly
            ]);
            var exercises = runner.GetExercises();

            while (true) {
                ShowExerciseOptions(exercises);
                var code = GetExerciseCode();

                if (code is "Q" or "q") {
                    break;
                }

                var current = SelectExerciseByCode(exercises, code);

                if (current == null) {
                    continue;
                }

                Console.Clear();
                Console.WriteLine($"Executing exercise: {current.Description}");
                Console.WriteLine();
                Task.Run(() => current.RunAsync(Console.Out, CancellationToken.None));
                Console.ReadKey();
            }

            Console.WriteLine("I'm out!");
        }

        private static ExerciseBase? SelectExerciseByCode(ExerciseBase[] exercises, string? code)
            => exercises
                .FirstOrDefault(exercise
                    => string.Equals(
                        a: exercise.Code,
                        b: code,
                        comparisonType: StringComparison.OrdinalIgnoreCase
                    ));

        private static string? GetExerciseCode() {
            Console.Write("Digite o código (ou 'Q' para sair): ");
            var code = Console.ReadLine();
            return code;
        }

        private static void ShowExerciseOptions(ExerciseBase[] exercises) {
            Console.Clear();
            Console.WriteLine("Selecione um dos exercícios da lista para executar:");
            Console.WriteLine();
            foreach (var exercise in exercises) {
                Console.WriteLine($"\t- [{exercise.Code}] {exercise.Description}");
            }
            Console.WriteLine();
        }

        #endregion
    }
}
