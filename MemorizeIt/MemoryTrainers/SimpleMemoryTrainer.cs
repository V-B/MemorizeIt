﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemorizeIt.MemoryStorage;
using MemorizeIt.Model;

namespace MemorizeIt.MemoryTrainers
{
    public class SimpleMemoryTrainer
    {
        private readonly IMemoryStorage storage;
        private readonly IRandomizer randomizer;
		public SimpleMemoryTrainer(IMemoryStorage storage){
			this.storage = storage;
			this.randomizer = new SimpleRandomizer (this.storage);
		}
        public SimpleMemoryTrainer(IMemoryStorage storage, IRandomizer randomizer)
        {
            this.storage = storage;
            this.randomizer = randomizer;
        }

        public void PickQuestion()
		{
			if (currentQuestionAndAnswer != null)
				throw new InvalidOperationException ("question is not empty, can't start new one");
            
			currentQuestionAndAnswer = this.randomizer.GetRandomUnsuccessItem ();

			if (currentQuestionAndAnswer == null)
				throw new InvalidOperationException ("no avalible questions for you");
		}

		public void Clear(){
			currentQuestionAndAnswer = null;
		}

        public bool Validate(string answer)
        {
            if(currentQuestionAndAnswer==null)
                throw new InvalidOperationException("question is empty");
            var result = Validate(answer, currentQuestionAndAnswer.Answer);
            if (result)
                this.storage.ItemSuccess(currentQuestionAndAnswer.Id);
            else
                this.storage.ItemFail(currentQuestionAndAnswer.Id);

            return result;
        }

        public QuestionAndAnswer CurrentQuestion
        {
            get
            {
                return currentQuestionAndAnswer;
            }
        }

		public bool IsQuestionsAvalible(){
			var testQuestion = randomizer.GetRandomUnsuccessItem ();
			return testQuestion != null;
		}

        private bool Validate(string answer, string correctAnswer)
        {
            return
                string.Compare(answer.Trim(), correctAnswer.Trim(), StringComparison.CurrentCultureIgnoreCase) ==
                0;
        }


        private QuestionAndAnswer currentQuestionAndAnswer;
    }
}
