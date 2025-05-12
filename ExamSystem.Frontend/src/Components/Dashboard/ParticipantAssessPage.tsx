import { useNavigate, useParams } from "react-router-dom";
import { useExams } from "../../Providers/ExamsProvider";
import {
  Stack,
  Typography,
  Box,
  Divider,
  Paper,
  Zoom,
  IconButton,
  TextField,
  Chip,
  FormControlLabel,
  Checkbox,
  Grid,
} from "@mui/material";
import { useEffect, useState } from "react";
import DashboardPaper from "../Papers/DashboardPaper";
import LoadingPage from "../Extra/LoadingPage";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import PrimaryFab from "../Buttons/PrimaryFab";
import { ExaminatorExam } from "../../Models/ExaminatorExam";
import { Participant } from "../../Models/Participant";
import QuestionService from "../../Services/QuestionService";
import AnswerService from "../../Services/AnswerService";
import { Answer } from "../../Models/Answer";
import PrimaryButton from "../Buttons/PrimaryButton";

const ParticipantAssessPage: React.FC = () => {
  const { examId, userId } = useParams<{ examId: string; userId: string }>();
  const { examinatorExams, questions, fetchQuestions, fetchExaminatorExams } =
    useExams();

  const [participant, setParticipant] = useState<Participant | null>(null);
  const [answers, setAnswers] = useState<Answer[] | null>(null);
  const [gradedAnswers, setGradedAnswers] = useState<Set<string>>(new Set());

  const [formValues, setFormValues] = useState({
    name: "",
    startDate: "",
    endDate: "",
    status: "",
  });

  const navigate = useNavigate();

  useEffect(() => {
    if (examId) {
      fetchQuestions(examId);
    }
  }, []);

  const exam: ExaminatorExam | undefined = examinatorExams?.find(
    (e) => e?.examId === examId
  );

  useEffect(() => {
    if (exam) {
      setFormValues({
        name: exam.name,
        startDate: exam.startDate,
        endDate: exam.endDate,
        status: exam.status,
      });

      if (userId) {
        setParticipant(
          examinatorExams
            ?.find((exam) => exam.examId === examId)
            ?.participants?.find((p) => p.userId === userId) || null
        );
      }
    }
  }, [exam]);

  useEffect(() => {
    const loadAnswers = async () => {
      if (participant) {
        const res = await AnswerService.getAllByExamUserId(
          participant.examUserId
        );
        setAnswers(res);
      }
    };
    loadAnswers();
  }, [participant]);
  const [grades, setGrades] = useState<Record<string, string>>({});
  const [errors, setErrors] = useState<Record<string, string>>({});

  const handleGradeQuestion = async (
    questionId: string,
    answerId: string,
    grade: number
  ) => {
    try {
      await QuestionService.gradeTextQuestion(questionId, {
        examUserId: participant!.examUserId,
        answerId,
        grade,
      });

      const updatedAnswers = await AnswerService.getAllByExamUserId(
        participant!.examUserId
      );
      setAnswers(updatedAnswers);

      setGradedAnswers((prev) => new Set(prev).add(answerId));
    } catch (error) {
      console.error("Failed to grade question:", error);
    }
  };

  if (!exam || !participant || !questions) {
    return <LoadingPage />;
  }

  return (
    <>
      <DashboardPaper
        sx={{ height: "calc(100vh - 100px)", overflowY: "hidden" }}
      >
        <Box
          display="flex"
          maxHeight="100%"
          width="100%"
          flexDirection={"column"}
        >
          <Paper sx={{ p: 2 }} elevation={2}>
            <Grid
              container
              direction="column"
              gridTemplateRows={"1fr 1fr 1fr"}
              spacing={2}
            >
              <Grid container spacing={2}>
                <Grid size={{ xs: 4 }}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Participant:
                  </Typography>
                  <Typography>
                    {participant.firstName} {participant.lastName}
                  </Typography>
                </Grid>
                <Grid size={{ xs: 4 }}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Email:
                  </Typography>
                  <Typography>{participant.email || "—"}</Typography>
                </Grid>
                <Grid size={{ xs: 4 }}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Status:
                  </Typography>
                  <Chip
                    sx={{ borderRadius: 1 }}
                    size="small"
                    label={
                      participant.completeStatus ? "Completed" : "Not Completed"
                    }
                    color={participant.completeStatus ? "success" : "info"}
                  />
                </Grid>
              </Grid>

              <Grid container spacing={2}>
                <Grid size={{ xs: 4 }}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Total Grade:
                  </Typography>
                  <Typography>
                    {participant.grade.toString()} /{" "}
                    {questions.reduce((t, q) => t + q.maxPoints, 0)}
                  </Typography>
                </Grid>
                <Grid size={{ xs: 4 }}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Answered Questions:
                  </Typography>
                  <Typography>
                    {answers
                      ? new Set(answers.map((a) => a.questionId)).size
                      : "0"}{" "}
                    / {questions.length}
                  </Typography>
                </Grid>
                <Grid size={{ xs: 4 }}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Is Graded:
                  </Typography>
                  <Chip
                    sx={{ borderRadius: 1 }}
                    size="small"
                    label={participant.isChecked ? "Yes" : "No"}
                    color={participant.isChecked ? "success" : "info"}
                  />
                </Grid>
              </Grid>
            </Grid>
          </Paper>
          <Box mt={2} flexGrow={1} overflow="auto">
            <Stack spacing={3} p={2}>
              {!answers && (
                <Paper sx={{ p: 2 }} elevation={2}>
                  <Typography variant="h6" gutterBottom>
                    No answers found for this participant.
                  </Typography>
                </Paper>
              )}
              {answers &&
                questions?.map((question) => {
                  const relatedAnswers = answers.filter(
                    (a) => a.questionId === question.questionId
                  );

                  return (
                    <Paper
                      key={question.questionId}
                      sx={{ p: 2 }}
                      elevation={2}
                    >
                      <Typography variant="h6" gutterBottom>
                        {question.questionText}
                      </Typography>
                      <Divider sx={{ mb: 2 }} />

                      {question.type === "Text" ? (
                        relatedAnswers.map((ans) => {
                          const grade = grades[ans.answerId] || "";
                          const error = errors[ans.answerId] || "";

                          const handleAssessClick = () => {
                            const numeric = parseFloat(grade);
                            if (
                              isNaN(numeric) ||
                              numeric < 0 ||
                              numeric > question.maxPoints
                            ) {
                              setErrors((prev) => ({
                                ...prev,
                                [ans.answerId]: `Enter number from 0 to ${question.maxPoints}`,
                              }));
                              return;
                            }

                            setErrors((prev) => ({
                              ...prev,
                              [ans.answerId]: "",
                            }));

                            handleGradeQuestion(
                              question.questionId,
                              ans.answerId,
                              numeric
                            );
                          };

                          return (
                            <Box key={ans.answerId} sx={{ mt: 1 }}>
                              <Typography sx={{ mb: 1 }}>
                                <strong>Answer:</strong> {ans.answerText || "—"}
                              </Typography>

                              {ans.isGraded ? (
                                <Typography
                                  color="success.main"
                                  fontWeight="bold"
                                >
                                  Graded!
                                </Typography>
                              ) : (
                                <Stack
                                  direction="row"
                                  alignItems="center"
                                  spacing={2}
                                >
                                  <TextField
                                    type="number"
                                    label="Points"
                                    value={grade}
                                    onChange={(e) =>
                                      setGrades((prev) => ({
                                        ...prev,
                                        [ans.answerId]: e.target.value,
                                      }))
                                    }
                                    error={!!error}
                                    helperText={error}
                                    inputProps={{
                                      min: 0,
                                      max: question.maxPoints,
                                    }}
                                    sx={{ width: 120 }}
                                  />
                                  <PrimaryButton onClick={handleAssessClick}>
                                    Grade
                                  </PrimaryButton>
                                </Stack>
                              )}
                            </Box>
                          );
                        })
                      ) : (
                        <>
                          {question.options.map((opt) => {
                            const ans = relatedAnswers.find(
                              (a) => a.questionOptionId === opt.questionOptionId
                            );

                            return (
                              <FormControlLabel
                                key={opt.questionOptionId}
                                control={<Checkbox checked={!!ans} disabled />}
                                label={`${opt.label}. ${opt.optionText}`}
                                sx={{ display: "block", ml: 1 }}
                              />
                            );
                          })}

                          <Typography variant="body2" sx={{ mt: 1 }}>
                            Points: {relatedAnswers[0]?.answerText || "—"}
                          </Typography>
                        </>
                      )}
                    </Paper>
                  );
                })}
            </Stack>
          </Box>
        </Box>
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
    </>
  );
};

export default ParticipantAssessPage;
