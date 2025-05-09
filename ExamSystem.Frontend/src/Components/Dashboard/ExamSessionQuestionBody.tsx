import React, { useState } from "react";
import {
  CircularProgress,
  FormControlLabel,
  TextField,
  Typography,
  Checkbox,
  FormGroup,
} from "@mui/material";
import { Question } from "../../Models/Question";
import DashboardPaper from "../Papers/DashboardPaper";
import PrimaryButton from "../Buttons/PrimaryButton";

type Props = {
  question: Question;
  onSubmit: (answer: { text?: string; optionIds?: string[] }) => void;
  isSubmitting: boolean;
};

const ExamSessionQuestionBody: React.FC<Props> = ({
  question,
  onSubmit,
  isSubmitting,
}) => {
  const [selectedOptionIds, setSelectedOptionIds] = useState<string[]>([]);
  const [textAnswer, setTextAnswer] = useState<string>("");

  const handleNextClick = () => {
  if (question.type === "Text") {
    if (!textAnswer.trim()) return;
    onSubmit({ text: textAnswer });
  } else {
    if (selectedOptionIds.length === 0) return;
    onSubmit({ optionIds: selectedOptionIds });
  }
};


  return (
    <DashboardPaper
      elevation={3}
      style={{ padding: 24, maxWidth: 800, margin: "0 auto" }}
    >
      <Typography variant="h6" gutterBottom>
        {question.questionText}
      </Typography>

      {question.type === "Text" ? (
        <TextField
          fullWidth
          multiline
          rows={4}
          value={textAnswer}
          onChange={(e) => setTextAnswer(e.target.value)}
          variant="outlined"
          label="Your answer"
        />
      ) : (
        <FormGroup>
          {question.options.map((option) => (
            <FormControlLabel
              key={option.questionOptionId}
              control={
                <Checkbox
                  checked={selectedOptionIds.includes(option.questionOptionId)}
                  onChange={(e) => {
                    const isChecked = e.target.checked;
                    setSelectedOptionIds((prev) =>
                      isChecked
                        ? [...prev, option.questionOptionId]
                        : prev.filter((id) => id !== option.questionOptionId)
                    );
                  }}
                />
              }
              label={option.optionText}
            />
          ))}
        </FormGroup>
      )}

      <div style={{ marginTop: 20 }}>
        <PrimaryButton
          variant="contained"
          color="primary"
          onClick={handleNextClick}
          disabled={isSubmitting}
        >
          {isSubmitting ? <CircularProgress size={24} /> : "Next"}
        </PrimaryButton>
      </div>
    </DashboardPaper>
  );
};

export default ExamSessionQuestionBody;
