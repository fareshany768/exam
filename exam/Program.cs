using System;
using System.Collections.Generic;
using System.Diagnostics;


class Answer
{
    public int Id { get; set; }
    public string Text { get; set; }

    public Answer(int id, string text)
    {
        Id = id;
        Text = text;
    }
}

abstract class Question
{
    public string Description { get; set; }
    public int Score { get; set; }
    public List<Answer> Options { get; set; }
    public Answer CorrectAnswer { get; set; }

    public Question(string description, int score, List<Answer> options, Answer correctAnswer)
    {
        Description = description;
        Score = score;
        Options = options;
        CorrectAnswer = correctAnswer;
    }

    public abstract void Display();
}


class TrueFalseQuestion : Question
{
    public TrueFalseQuestion(string description, int score, Answer correctAnswer)
        : base(description, score, new List<Answer> { new Answer(1, "True"), new Answer(2, "False") }, correctAnswer) { }

    public override void Display()
    {
        Console.WriteLine($"{Description} ({Score} Points)");
        Console.WriteLine("1) True\n2) False");
    }
}

class MCQQuestion : Question
{
    public MCQQuestion(string description, int score, List<Answer> options, Answer correctAnswer)
        : base(description, score, options, correctAnswer) { }

    public override void Display()
    {
        Console.WriteLine($"{Description} ({Score} Points)");
        for (int i = 0; i < Options.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {Options[i].Text}");
        }
    }
}

abstract class Exam
{
    public int Duration { get; set; }
    public int TotalQuestions { get; set; }
    public List<Question> QuestionList { get; set; }
    public int Score { get; private set; }

    public Exam(int duration, int totalQuestions)
    {
        Duration = duration;
        TotalQuestions = totalQuestions;
        QuestionList = new List<Question>();
        Score = 0;
    }

    public abstract void DisplayExam();

    public void TakeExam()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        foreach (var question in QuestionList)
        {
            question.Display();
            Console.Write("Your answer: ");
            int userAnswer = int.Parse(Console.ReadLine());
            if (question.Options[userAnswer - 1].Id == question.CorrectAnswer.Id)
            {
                Console.WriteLine("Correct!");
                Score += question.Score;
            }
            else
            {
                Console.WriteLine($"Wrong! Correct answer: {question.CorrectAnswer.Text}");
            }
            Console.WriteLine();
        }
        stopwatch.Stop();
        Console.WriteLine($"Your total score: {Score}");
        Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalSeconds} seconds");
    }
}


class FinalExam : Exam
{
    public FinalExam(int duration, int totalQuestions) : base(duration, totalQuestions) { }

    public override void DisplayExam()
    {
        Console.WriteLine("Final Exam");
        TakeExam();
    }
}


class PracticalExam : Exam
{
    public PracticalExam(int duration, int totalQuestions) : base(duration, totalQuestions) { }

    public override void DisplayExam()
    {
        Console.WriteLine("Practical Exam");
        TakeExam();
    }
}


class Program
{
    static void Main()
    {
        Console.Write("Do you want to start the exam? (yes/no): ");
        string startExam = Console.ReadLine().ToLower();
        if (startExam != "yes") return;

        Console.WriteLine("Select Exam Type: 1) Final Exam  2) Practical Exam");
        int examChoice = int.Parse(Console.ReadLine());

        Console.Write("Enter exam duration (minutes): ");
        int duration = int.Parse(Console.ReadLine());

        Console.Write("Enter number of questions: ");
        int numQuestions = int.Parse(Console.ReadLine());

        Exam exam = examChoice == 1 ? new FinalExam(duration, numQuestions) : new PracticalExam(duration, numQuestions);

        for (int i = 0; i < numQuestions; i++)
        {
            int questionType;
            if (examChoice == 1)
            {
                Console.WriteLine("Select Question Type: 1) True/False  2) MCQ");
                questionType = int.Parse(Console.ReadLine());
            }
            else
            {
                questionType = 2; 
            }

            Console.Write("Enter question body: ");
            string body = Console.ReadLine();

            Console.Write("Enter question score: ");
            int score = int.Parse(Console.ReadLine());

            if (questionType == 1)
            {
                Console.Write("Enter correct answer (1 for True, 2 for False): ");
                int correctId = int.Parse(Console.ReadLine());
                Answer correctAnswer = new Answer(correctId, correctId == 1 ? "True" : "False");
                exam.QuestionList.Add(new TrueFalseQuestion(body, score, correctAnswer));
            }
            else
            {
                List<Answer> options = new List<Answer>();
                for (int j = 1; j <= 3; j++)
                {
                    Console.Write($"Enter option {j}: ");
                    options.Add(new Answer(j, Console.ReadLine()));
                }
                Console.Write("Enter correct answer (1-3): ");
                int correctId = int.Parse(Console.ReadLine());
                Answer correctAnswer = options[correctId - 1];
                exam.QuestionList.Add(new MCQQuestion(body, score, options, correctAnswer));
            }
        }

        exam.DisplayExam();
    }
}

