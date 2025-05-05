import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Stack,
  Checkbox,
  FormControlLabel,
} from "@mui/material";
import { useState } from "react";
import { QuestionCreate } from "../../Models/QuestionCreate";
import { QuestionOptionCreate } from "../../Models/QuestionOptionCreate";

interface Props {
  open: boolean;
  onClose: () => void;
  onSave: (question: QuestionCreate) => void;
  examId: string;
}

const CreateQuestionModal: React.FC<Props> = ({
  open,
  onClose,
  onSave,
  examId,
}) => {
  const [question, setQuestion] = useState<QuestionCreate>({
    examId,
    questionText: "",
    type: "",
    imageUrl: "",
    options: [],
  });

  const [option, setOption] = useState<QuestionOptionCreate>({
    label: "",
    optionText: "",
    isCorrect: false,
  });

  const handleQuestionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setQuestion({ ...question, [e.target.name]: e.target.value });
  };

  const handleOptionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, checked, type } = e.target;
    setOption({
      ...option,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const addOption = () => {
    setQuestion((prev) => ({
      ...prev,
      options: [...prev.options, option],
    }));
    setOption({ label: "", optionText: "", isCorrect: false });
  };

  const saveQuestion = () => {
    onSave(question);
    onClose();
    setQuestion({
      examId,
      questionText: "",
      type: "",
      imageUrl: "",
      options: [],
    });
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Create Question</DialogTitle>
      <DialogContent>
        <Stack spacing={2} mt={1}>
          <TextField
            label="Question Text"
            name="questionText"
            fullWidth
            onChange={handleQuestionChange}
          />
          <TextField
            label="Type"
            name="type"
            fullWidth
            onChange={handleQuestionChange}
          />
          <TextField
            label="Image URL"
            name="imageUrl"
            fullWidth
            onChange={handleQuestionChange}
          />

          <TextField
            label="Option Label"
            name="label"
            fullWidth
            value={option.label}
            onChange={handleOptionChange}
          />
          <TextField
            label="Option Text"
            name="optionText"
            fullWidth
            value={option.optionText}
            onChange={handleOptionChange}
          />
          <FormControlLabel
            control={
              <Checkbox
                checked={option.isCorrect}
                onChange={handleOptionChange}
                name="isCorrect"
              />
            }
            label="Correct Answer"
          />
          <Button onClick={addOption}>Add Option</Button>

          {question.options.map((opt, index) => (
            <div key={index}>
              {opt.label}: {opt.optionText} {opt.isCorrect ? "(correct)" : ""}
            </div>
          ))}
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button variant="contained" onClick={saveQuestion}>
          Save Question
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default CreateQuestionModal;
