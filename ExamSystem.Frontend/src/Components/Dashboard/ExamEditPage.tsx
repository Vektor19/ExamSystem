import { useNavigate, useParams } from "react-router-dom";
import { useExams } from "../../Providers/ExamsProvider";
import { Exam } from "../../Models/Exam";
import {
  Stack,
  Typography,
  Box,
  Divider,
  Paper,
  Zoom,
  IconButton,
  TextField,
  Select,
  MenuItem,
  Button,
} from "@mui/material";
import { QuestionCreate } from "../../Models/QuestionCreate";
import { useEffect, useState } from "react";
import CreateQuestionModal from "./CreateQuestionModal";
import DashboardPaper from "../Papers/DashboardPaper";
import LoadingPage from "../Extra/LoadingPage";
import AddIcon from "@mui/icons-material/Add";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import EditIcon from "@mui/icons-material/Edit";
import CheckIcon from "@mui/icons-material/Check";
import CloseIcon from "@mui/icons-material/Close";
import PrimaryFab from "../Buttons/PrimaryFab";
import QuestionService from "../../Services/QuestionService";
import ExamService from "../../Services/ExamService";

const statusOptions = ["NotStarted", "Started", "Finished"];

const ExamEditPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { examinatorExams, questions, fetchQuestions } = useExams();
  const [showQuestionModal, setShowQuestionModal] = useState(false);
  const [editingField, setEditingField] = useState<string | null>(null);
  const [formValues, setFormValues] = useState({
    name: "",
    startDate: "",
    endDate: "",
    status: "",
  });

  const navigate = useNavigate();

  useEffect(() => {
    if (id) {
      fetchQuestions(id);
    }
  }, []);

  const exam: Exam | undefined = examinatorExams?.find((e) => e?.examId === id);

  useEffect(() => {
    if (exam) {
      setFormValues({
        name: exam.name,
        startDate: exam.startDate,
        endDate: exam.endDate,
        status: exam.status,
      });
    }
  }, [exam]);

  const handleSaveField = async (field: keyof typeof formValues) => {
    if (!exam) return;
    try {
      //   await ExamService.updateExam({
      //     name: formValues.name,
      //     startDate: new Date(formValues.startDate),
      //     endDate: new Date(formValues.endDate),
      //     status: formValues.status,
      //   });
      setEditingField(null);
    } catch (error) {
      console.error("Failed to update exam:", error);
    } finally {
      setFormValues(exam);
    }
  };

  const renderField = (
    label: string,
    field: keyof typeof formValues,
    isDate = false,
    isSelect = false
  ) => {
    const value = formValues[field];
    const isEditing = editingField === field;
    return (
      <Box display="flex" alignItems="center" gap={1}>
        <strong>{label}:</strong>
        {isEditing ? (
          isSelect ? (
            <Select
              size="small"
              value={value}
              onChange={(e) =>
                setFormValues((prev) => ({ ...prev, [field]: e.target.value }))
              }
            >
              {statusOptions.map((status) => (
                <MenuItem key={status} value={status}>
                  {status}
                </MenuItem>
              ))}
            </Select>
          ) : (
            <TextField
              size="small"
              type={isDate ? "date" : "text"}
              value={isDate ? value.slice(0, 10) : value}
              onChange={(e) =>
                setFormValues((prev) => ({ ...prev, [field]: e.target.value }))
              }
            />
          )
        ) : (
          <Typography>{exam?.[field]}</Typography>
        )}
        {isEditing ? (
          <>
            <IconButton
              onClick={() => handleSaveField(field)}
              size="small"
              color="success"
            >
              <CheckIcon />
            </IconButton>
            <IconButton
              onClick={() => {
                setEditingField(null);
                setFormValues(
                  exam ?? { name: "", startDate: "", endDate: "", status: "" }
                );
              }}
              size="small"
              color="error"
            >
              <CloseIcon />
            </IconButton>
          </>
        ) : (
          <IconButton onClick={() => setEditingField(field)} size="small">
            <EditIcon />
          </IconButton>
        )}
      </Box>
    );
  };

  if (!exam) {
    return <LoadingPage />;
  }

  return (
    <>
      <CreateQuestionModal
        open={showQuestionModal}
        onClose={() => setShowQuestionModal(false)}
        onSave={async (question) => {
          await QuestionService.createQuestion(question);
          await fetchQuestions(question.examId);
          setShowQuestionModal(false);
        }}
        examId={exam?.examId || ""}
      />

      <DashboardPaper sx={{ height: "calc(100vh - 100px)", overflowY: "auto" }}>
        <Stack spacing={4} mt={2} px={3} pb={10}>
          <Typography variant="h6" fontWeight={700}>
            Edit Exam: {exam?.name}
          </Typography>

          <Box display="grid" gridTemplateColumns="1fr 1fr" gap={2}>
            {renderField("Name", "name")}
            {renderField("Start Date", "startDate", true)}
            {renderField("End Date", "endDate", true)}
            <Typography>
              <strong>Questions:</strong> {exam?.questionCount}
            </Typography>
            <Typography>
              <strong>Participants:</strong> {exam?.participantCount}
            </Typography>
            {renderField("Status", "status", false, true)}
            <Typography>
              <strong>Join Code:</strong> {exam?.joinCode}
            </Typography>
          </Box>

          <Divider />

          <Box>
            <Typography variant="subtitle1" fontWeight={600} mb={2}>
              Questions ({questions?.length})
            </Typography>
            <Stack spacing={2}>
              {questions?.map((q, i) => (
                <Paper key={i} sx={{ p: 2 }}>
                  <Typography variant="subtitle1" fontWeight={500}>
                    {i + 1}. {q.questionText}
                  </Typography>
                  <Stack component="ul" pl={2} spacing={0.5}>
                    {q.options.map((option, idx) => (
                      <li key={idx}>
                        <Typography variant="body2">
                          {option?.label}: {option?.optionText}
                        </Typography>
                      </li>
                    ))}
                  </Stack>
                </Paper>
              ))}
              {questions?.length === 0 && (
                <Typography color="text.secondary">
                  No questions created yet.
                </Typography>
              )}
            </Stack>
          </Box>
        </Stack>
      </DashboardPaper>

      <Zoom in>
        <Box
          position="fixed"
          bottom={24}
          left={"calc(var(--dashboard-navigation-width) + 24px)"}
          zIndex={1300}
        >
          <PrimaryFab
            size="small"
            variant="extended"
            onClick={() => navigate(-1)}
          >
            <ArrowBackIcon sx={{ mr: 1 }} />
            Go Back
          </PrimaryFab>
        </Box>
      </Zoom>

      <Zoom in>
        <Box position="fixed" bottom={24} right={24} zIndex={1300}>
          <PrimaryFab
            size="small"
            variant="extended"
            onClick={() => setShowQuestionModal(true)}
          >
            <AddIcon sx={{ mr: 1 }} />
            Add Question
          </PrimaryFab>
        </Box>
      </Zoom>
    </>
  );
};

export default ExamEditPage;
