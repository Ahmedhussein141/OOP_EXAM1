using System;

namespace OOP_EXAM
{
    class Answer
    {
        public int AnswerId;
        public string AnswerText;

        public Answer(int id, string text)
        {
            AnswerId = id;
            AnswerText = text;
        }

        public override string ToString()
        {
            return AnswerId + ". " + AnswerText;
        }
    }

    abstract class Question
    {
        public string Header;
        public string Body;
        public int Mark;
        public Answer[] Answers;
        public Answer RightAnswer;

        public abstract void ShowQuestion();
    }

    class TrueFalseQuestion : Question
    {
        public TrueFalseQuestion(string header, string body, int mark, bool correct)
        {
            Header = header;
            Body = body;
            Mark = mark;
            Answers = new Answer[2];
            Answers[0] = new Answer(1, "True");
            Answers[1] = new Answer(2, "False");
            RightAnswer = correct ? Answers[0] : Answers[1];
        }

        public override void ShowQuestion()
        {
            Console.WriteLine(Header);
            Console.WriteLine(Body);
            for (int i = 0; i < Answers.Length; i++)
            {
                Console.WriteLine(Answers[i].ToString());
            }
            Console.Write("Your answer: ");
        }
    }

    class MCQQuestion : Question
    {
        public MCQQuestion(string header, string body, int mark, Answer[] answers, int rightId)
        {
            Header = header;
            Body = body;
            Mark = mark;
            Answers = answers;
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i].AnswerId == rightId)
                {
                    RightAnswer = answers[i];
                }
            }
        }

        public override void ShowQuestion()
        {
            Console.WriteLine(Header);
            Console.WriteLine(Body);
            for (int i = 0; i < Answers.Length; i++)
            {
                Console.WriteLine(Answers[i].ToString());
            }
            Console.Write("Your answer: ");
        }
    }

    abstract class Exam
    {
        public int Time;
        public int NumberOfQuestions;
        public Question[] Questions;

        public abstract void ShowExam();
    }

    class FinalExam : Exam
    {
        public FinalExam(int time, int n)
        {
            Time = time;
            NumberOfQuestions = n;
            Questions = new Question[n];
        }

        public override void ShowExam()
        {
            int score = 0;
            int total = 0;
            int[] chosen = new int[Questions.Length];

            for (int i = 0; i < Questions.Length; i++)
            {
                Questions[i].ShowQuestion();
                int ans;
                int.TryParse(Console.ReadLine(), out ans);
                chosen[i] = ans;

                if (Questions[i].RightAnswer != null && ans == Questions[i].RightAnswer.AnswerId)
                {
                    score += Questions[i].Mark;
                }
                total += Questions[i].Mark;
            }

            Console.WriteLine("=== Review ===");
            for (int i = 0; i < Questions.Length; i++)
            {
                Console.WriteLine(Questions[i].Header);
                Console.WriteLine(Questions[i].Body);
                for (int j = 0; j < Questions[i].Answers.Length; j++)
                {
                    Console.WriteLine(Questions[i].Answers[j].ToString());
                }
                Console.WriteLine("Your answer: " + chosen[i]);
                Console.WriteLine("Right answer: " + Questions[i].RightAnswer.AnswerId);
            }
            Console.WriteLine("Grade: " + score + "/" + total);
        }
    }

    class PracticalExam : Exam
    {
        public PracticalExam(int time, int n)
        {
            Time = time;
            NumberOfQuestions = n;
            Questions = new Question[n];
        }

        public override void ShowExam()
        {
            int score = 0;
            int total = 0;
            int[] chosen = new int[Questions.Length];

            for (int i = 0; i < Questions.Length; i++)
            {
                Questions[i].ShowQuestion();
                int ans;
                int.TryParse(Console.ReadLine(), out ans);
                chosen[i] = ans;

                if (Questions[i].RightAnswer != null && ans == Questions[i].RightAnswer.AnswerId)
                {
                    score += Questions[i].Mark;
                }
                total += Questions[i].Mark;
            }

            Console.WriteLine("=== Correct Answers ===");
            for (int i = 0; i < Questions.Length; i++)
            {
                Console.WriteLine(Questions[i].Header + " -> " + Questions[i].RightAnswer.AnswerId);
            }
            Console.WriteLine("Your score: " + score + "/" + total);
        }
    }

    class Subject
    {
        public int SubjectId;
        public string SubjectName;
        public Exam Exam;

        public Subject(int id, string name)
        {
            SubjectId = id;
            SubjectName = name;
        }

        public void CreateExam()
        {
            Console.Write("Enter exam time: ");
            int time = int.Parse(Console.ReadLine());
            Console.Write("Exam type (1 Final, 2 Practical): ");
            int type = int.Parse(Console.ReadLine());
            Console.Write("Number of questions: ");
            int n = int.Parse(Console.ReadLine());

            if (type == 1)
            {
                Exam = new FinalExam(time, n);
            }
            else
            {
                Exam = new PracticalExam(time, n);
            }

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("Question " + (i + 1));
                Console.Write("Type (1 TF, 2 MCQ): ");
                int qt = int.Parse(Console.ReadLine());
                Console.Write("Header: ");
                string h = Console.ReadLine();
                Console.Write("Body: ");
                string b = Console.ReadLine();
                Console.Write("Mark: ");
                int m = int.Parse(Console.ReadLine());

                if (qt == 1)
                {
                    Console.Write("Correct (1 True, 2 False): ");
                    int cor = int.Parse(Console.ReadLine());
                    bool correct = cor == 1;
                    Exam.Questions[i] = new TrueFalseQuestion(h, b, m, correct);
                }
                else
                {
                    Console.Write("How many choices: ");
                    int c = int.Parse(Console.ReadLine());
                    Answer[] arr = new Answer[c];
                    for (int j = 0; j < c; j++)
                    {
                        Console.Write("Choice " + (j + 1) + ": ");
                        string txt = Console.ReadLine();
                        arr[j] = new Answer(j + 1, txt);
                    }
                    Console.Write("Right choice id: ");
                    int rid = int.Parse(Console.ReadLine());
                    Exam.Questions[i] = new MCQQuestion(h, b, m, arr, rid);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter subject id: ");
            int sid = int.Parse(Console.ReadLine());
            Console.Write("Enter subject name: ");
            string sname = Console.ReadLine();

            Subject sub = new Subject(sid, sname);
            sub.CreateExam();

            Console.WriteLine("Starting exam...");
            sub.Exam.ShowExam();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
