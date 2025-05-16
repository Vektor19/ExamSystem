import { useExams } from "../../Providers/ExamsProvider";
import { useNavigate } from "react-router-dom";
import {
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Stack,
  Chip,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import PinIcon from "@mui/icons-material/PushPin";
import UnPinIcon from "@mui/icons-material/PushPinOutlined";
import PrimaryFab from "../Buttons/PrimaryFab";
import TimeUtils from "../../Utils/TimeUtils";
import DashboardPaper from "../Papers/DashboardPaper";
import { useEffect, useState } from "react";
import { ExaminatorExam } from "../../Models/ExaminatorExam";
import LoadingPage from "../Extra/LoadingPage";

const ExaminatorExamsBody: React.FC = () => {
  const {
    examinatorExams,
    pinnedExaminatorExams,
    addPinnedExaminatorExam,
    removePinnedExaminatorExam,
    fetchPinnedExaminatorExams,
  } = useExams();
  const navigate = useNavigate();
  const [sortedExams, setSortedExams] = useState<ExaminatorExam[]>([]);
  useEffect(() => {
    if (!examinatorExams) return;
    fetchPinnedExaminatorExams();
  }, [examinatorExams]);

  useEffect(() => {
    if (!examinatorExams) return;
    if (!pinnedExaminatorExams) return;
    const sorted = [...examinatorExams].sort((a, b) => {
      const isPinnedA = pinnedExaminatorExams.some(
        (exam) => exam.examId === a.examId
      );
      const isPinnedB = pinnedExaminatorExams.some(
        (exam) => exam.examId === b.examId
      );
      return isPinnedA === isPinnedB ? 0 : isPinnedA ? -1 : 1;
    });

    setSortedExams(sorted);
  }, [pinnedExaminatorExams]);

  if (!sortedExams) {
    return <LoadingPage />;
  }

  return (
    <>
      <DashboardPaper
        elevation={3}
        sx={{ p: 4, borderRadius: 3, overflowY: "auto" }}
      >
        <Stack spacing={2}>
          {sortedExams?.length ? (
            <TableContainer
              sx={{
                width: "100%",
                overflowX: "auto",
              }}
            >
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell sx={{ fontWeight: "bold" }}>Name</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>Status</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>
                      Start Date
                    </TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>End Date</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>Questions</TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>
                      Participants
                    </TableCell>
                    <TableCell sx={{ fontWeight: "bold" }}>Join Code</TableCell>
                    <TableCell align="right" sx={{ fontWeight: "bold" }}>
                      Actions
                    </TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {sortedExams.map(
                    (exam) =>
                      exam && (
                        <TableRow
                          key={exam.examId}
                          sx={
                            pinnedExaminatorExams?.some(
                              (e) => e.examId === exam.examId
                            )
                              ? { backgroundColor: "rgba(25, 118, 210, 0.05)" }
                              : {}
                          }
                        >
                          <TableCell>{exam.name}</TableCell>
                          <TableCell>
                            <Chip
                              sx={{ borderRadius: 1 }}
                              size="small"
                              label={exam.status}
                              color={
                                exam.status === "Closed"
                                  ? "success"
                                  : exam.status === "Started"
                                  ? "primary"
                                  : "warning"
                              }
                            />
                          </TableCell>
                          <TableCell>
                            {TimeUtils.formatDate(exam.startDate)}
                          </TableCell>
                          <TableCell>
                            {TimeUtils.formatDate(exam.endDate)}
                          </TableCell>
                          <TableCell>{exam.questionCount}</TableCell>
                          <TableCell>{exam.participantCount}</TableCell>
                          <TableCell>{exam.joinCode}</TableCell>
                          <TableCell align="right">
                            <IconButton
                              onClick={() =>
                                navigate(`/dashboard/edit-exam/${exam.examId}`)
                              }
                              color="primary"
                            >
                              <EditIcon />
                            </IconButton>
                            <IconButton
                              onClick={() => {
                                const isPinned = pinnedExaminatorExams?.some(
                                  (e) => e.examId === exam.examId
                                );
                                if (isPinned) {
                                  removePinnedExaminatorExam(exam.examId);
                                } else {
                                  addPinnedExaminatorExam(exam.examId);
                                }
                              }}
                              color="primary"
                            >
                              {pinnedExaminatorExams?.some(
                                (e) => e.examId === exam.examId
                              ) ? (
                                <PinIcon sx={{ color: "primary.main" }} />
                              ) : (
                                <UnPinIcon sx={{ color: "primary.main" }} />
                              )}
                            </IconButton>
                          </TableCell>
                        </TableRow>
                      )
                  )}
                </TableBody>
              </Table>
            </TableContainer>
          ) : (
            <Typography variant="body1" color="text.secondary">
              No exams found.
            </Typography>
          )}
        </Stack>
      </DashboardPaper>

      <PrimaryFab
        color="primary"
        aria-label="add"
        size="large"
        sx={{
          position: "fixed",
          bottom: 24,
          right: 24,
          zIndex: 1000,
        }}
        onClick={() => navigate("/dashboard/create-exam")}
      >
        <AddIcon />
      </PrimaryFab>
    </>
  );
};

export default ExaminatorExamsBody;
