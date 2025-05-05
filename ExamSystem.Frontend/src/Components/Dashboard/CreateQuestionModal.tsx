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
  Typography,
  Divider,
  Paper,
  Box,
} from "@mui/material";
import { useState } from "react";
import { QuestionCreate } from "../../Models/QuestionCreate";
import { QuestionOptionCreate } from "../../Models/QuestionOptionCreate";
import PrimaryButton from "../Buttons/PrimaryButton";
import SecondaryButton from "../Buttons/SecondaryButton";

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
    if (option.label.trim() && option.optionText.trim()) {
      setQuestion((prev) => ({
        ...prev,
        options: [...prev.options, option],
      }));
      setOption({ label: "", optionText: "", isCorrect: false });
    }
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
        <Box display="flex" gap={3}>
          <Box flex={1}>
            <Stack spacing={3}>
              <Stack spacing={2}>
                <Typography variant="subtitle1" fontWeight="bold">
                  Question Info
                </Typography>
                <TextField
                  label="Question Text"
                  name="questionText"
                  fullWidth
                  value={question.questionText}
                  onChange={handleQuestionChange}
                />
                <TextField
                  label="Type"
                  name="type"
                  fullWidth
                  value={question.type}
                  onChange={handleQuestionChange}
                />
                <TextField
                  label="Image URL"
                  name="imageUrl"
                  fullWidth
                  value={question.imageUrl}
                  onChange={handleQuestionChange}
                />
              </Stack>

              <Divider />

              <Stack spacing={2}>
                <Typography variant="subtitle1" fontWeight="bold">
                  Add Option
                </Typography>
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
                <PrimaryButton variant="contained" onClick={addOption}>
                  Add Option
                </PrimaryButton>
              </Stack>
            </Stack>
          </Box>

          {/* Right side: Current options */}
          <Box
            flex={1}
            maxHeight="500px"
            overflow="auto"
            borderLeft="1px solid #ccc"
            pl={2}
          >
            <Typography variant="subtitle1" fontWeight="bold" mb={2}>
              Current Options
            </Typography>
            <Stack spacing={1}>
              {question.options.map((opt, index) => (
                <Paper
                  key={index}
                  variant="outlined"
                  sx={{ p: 1.5, backgroundColor: "#f9f9f9" }}
                >
                  <Typography>
                    <strong>{opt.label}:</strong> {opt.optionText}{" "}
                    {opt.isCorrect && (
                      <Typography
                        component="span"
                        color="success.main"
                        fontWeight="bold"
                      >
                        (Correct)
                      </Typography>
                    )}
                  </Typography>
                </Paper>
              ))}
            </Stack>
          </Box>
        </Box>
      </DialogContent>

      <DialogActions>
        <SecondaryButton onClick={onClose}>Cancel</SecondaryButton>
        <PrimaryButton variant="contained" onClick={saveQuestion}>
          Save Question
        </PrimaryButton>
      </DialogActions>
    </Dialog>
  );
};

export default CreateQuestionModal;
