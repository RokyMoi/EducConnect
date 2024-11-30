using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Enums;

namespace backend.Utilities
{
    public class TutorRegistrationStepsTitleUtility
    {
        public static readonly Dictionary<TutorRegistrationStepEnum, string> TutorRegistrationStepsTitles = new() {
        { TutorRegistrationStepEnum.Step1, "Email and Password" },
        { TutorRegistrationStepEnum.Step2, "Personal details" },
        { TutorRegistrationStepEnum.Step3, "Education" },
        { TutorRegistrationStepEnum.Step4, "Experience" },
        { TutorRegistrationStepEnum.Step5, "Teaching details" },
        { TutorRegistrationStepEnum.Step6, "Time organization and availability" },
        { TutorRegistrationStepEnum.Step7, "Financial details" },
        { TutorRegistrationStepEnum.Step8, "Review and complete" }
        };

        public static string GetTitleByTutorRegistrationStep(TutorRegistrationStepEnum step)
        {
            return TutorRegistrationStepsTitles.TryGetValue(step, out var title) ? title : "Unknown step";
        }
    }
}