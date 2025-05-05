import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
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

type OptionLabelType = "number" | "letter" | "roman";

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
    type: "Text",
    imageUrl: "",
    options: [],
  });

  const [option, setOption] = useState<QuestionOptionCreate>({
    label: "",
    optionText: "",
    isCorrect: false,
  });

  const [labelType, setLabelType] = useState<OptionLabelType>("number");

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

  const toRoman = (num: number): string => {
    const romanMap: { [key: number]: string } = {
      1000: "M",
      900: "CM",
      500: "D",
      400: "CD",
      100: "C",
      90: "XC",
      50: "L",
      40: "XL",
      10: "X",
      9: "IX",
      5: "V",
      4: "IV",
      1: "I",
    };
    let result = "";
    for (const value of Object.keys(romanMap)
      .map(Number)
      .sort((a, b) => b - a)) {
      while (num >= value) {
        result += romanMap[value];
        num -= value;
      }
    }
    return result;
  };

  const getLabelByType = (type: OptionLabelType, index: number): string => {
    switch (type) {
      case "number":
        return (index + 1).toString();
      case "letter":
        return String.fromCharCode(97 + index); // a, b, c
      case "roman":
        return toRoman(index + 1);
      default:
        return (index + 1).toString();
    }
  };

  const addOption = () => {
    if (option.optionText.trim()) {
      const newIndex = question.options.length;
      const autoLabel = getLabelByType(labelType, newIndex);

      const newOption: QuestionOptionCreate = {
        ...option,
        label: autoLabel,
      };

      setQuestion((prev) => ({
        ...prev,
        options: [...prev.options, newOption],
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
      type: "Text",
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
              {/* --- Question Info --- */}
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
                  select
                  label="Question Type"
                  name="type"
                  fullWidth
                  value={question.type}
                  onChange={handleQuestionChange}
                  SelectProps={{ native: true }}
                >
                  <option value="Text">Text</option>
                  <option value="MultiChoice">MultiChoice</option>
                </TextField>
                <TextField
                  label="Image URL"
                  name="imageUrl"
                  fullWidth
                  value={question.imageUrl}
                  onChange={handleQuestionChange}
                />
              </Stack>

              {/* --- Option controls --- */}
              {question.type === "MultiChoice" && (
                <>
                  <TextField
                    select
                    label="Option Label Type"
                    fullWidth
                    value={labelType}
                    onChange={(e) =>
                      setLabelType(e.target.value as OptionLabelType)
                    }
                    SelectProps={{ native: true }}
                  >
                    <option value="number">1, 2, 3</option>
                    <option value="letter">a, b, c</option>
                    <option value="roman">I, II, III</option>
                  </TextField>

                  <Divider />

                  <Stack spacing={2}>
                    <Typography variant="subtitle1" fontWeight="bold">
                      Add Option
                    </Typography>
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
                </>
              )}
            </Stack>
          </Box>

          {/* Right side: Current options (only if MultiChoice) */}
          {question.type === "MultiChoice" && (
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
          )}
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
