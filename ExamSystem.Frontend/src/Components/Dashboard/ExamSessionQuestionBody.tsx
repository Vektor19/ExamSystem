import React, { useState } from "react";
import {
  Button,
  CircularProgress,
  Paper,
  Radio,
  RadioGroup,
  FormControlLabel,
  TextField,
  Typography,
} from "@mui/material";
import { Question } from "../../Models/Question";

type Props = {
  question: Question;
  onSubmit: (answer: { text?: string; optionId?: string }) => void;
  isSubmitting: boolean;
};

const ExamSessionQuestionBody: React.FC<Props> = ({ question, onSubmit, isSubmitting }) => {
  const [selectedOptionId, setSelectedOptionId] = useState<string>("");
  const [textAnswer, setTextAnswer] = useState<string>("");

  const handleNextClick = () => {
    if (question.type === "Text") {
      if (!textAnswer.trim()) return;
      onSubmit({ text: textAnswer });
    } else {
      if (!selectedOptionId) return;
      onSubmit({ optionId: selectedOptionId });
    }
  };

  return (
    <Paper elevation={3} style={{ padding: 24, maxWidth: 800, margin: "0 auto" }}>
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
        <RadioGroup
          value={selectedOptionId}
          onChange={(e) => setSelectedOptionId(e.target.value)}
        >
          {question.options.map((option) => (
            <FormControlLabel
              key={option.questionOptionId}
              value={option.questionOptionId}
              control={<Radio />}
              label={option.optionText}
            />
          ))}
        </RadioGroup>
      )}

      <div style={{ marginTop: 20 }}>
        <Button
          variant="contained"
          color="primary"
          onClick={handleNextClick}
          disabled={isSubmitting}
        >
          {isSubmitting ? <CircularProgress size={24} /> : "Next"}
        </Button>
      </div>
    </Paper>
  );
};

export default ExamSessionQuestionBody;
