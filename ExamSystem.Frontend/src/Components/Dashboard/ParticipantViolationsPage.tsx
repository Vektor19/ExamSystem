import { useNavigate, useParams } from "react-router-dom";
import { useExams } from "../../Providers/ExamsProvider";
import { Stack, Typography, Box, Divider, Paper, Zoom } from "@mui/material";
import { useEffect, useState } from "react";
import DashboardPaper from "../Papers/DashboardPaper";
import LoadingPage from "../Extra/LoadingPage";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import PrimaryFab from "../Buttons/PrimaryFab";
import { ExaminatorExam } from "../../Models/ExaminatorExam";
import { Participant } from "../../Models/Participant";
import { Violation } from "../../Models/Violation";
import ViolationService from "../../Services/ViolationService";

const ParticipantViolationsPage: React.FC = () => {
  const { examId, userId } = useParams<{ examId: string; userId: string }>();
  const { examinatorExams } = useExams();

  const [participant, setParticipant] = useState<Participant | null>(null);
  const [violations, setViolations] = useState<Violation[] | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    if (examId) {
    }
  }, []);

  const exam: ExaminatorExam | undefined = examinatorExams?.find(
    (e) => e?.examId === examId
  );

  useEffect(() => {
    if (userId) {
      setParticipant(
        examinatorExams
          ?.find((exam) => exam.examId === examId)
          ?.participants?.find((p) => p.userId === userId) || null
      );
    }
  }, [exam]);

  useEffect(() => {
    const fetchViolations = async () => {
      if (participant) {
        try {
          const violationsData = await ViolationService.getAllByExamUserId(
            participant.examUserId
          );
          setViolations(violationsData);
        } catch (error) {
          console.error("Error fetching violations:", error);
        }
      }
    };
    fetchViolations();
  }, [participant]);

  if (!exam || !participant) {
    return <LoadingPage />;
  }

  return (
    <>
      <Stack direction="row" alignItems="center" spacing={2} mb={2}>
        <Typography variant="h5" fontWeight={600} gutterBottom>
          Participant:
        </Typography>
        <Typography variant="h5" fontWeight={600} gutterBottom>
          {participant.lastName} {participant.firstName}
        </Typography>
      </Stack>
      <DashboardPaper sx={{ p: 3, overflowY: "auto" }}>
        <Stack spacing={2}>
          {(!violations || violations.length === 0) && (
            <Paper>
              <Stack direction="column" spacing={2} mt={1} flexWrap="wrap">
                <Typography variant="subtitle1" color="text.secondary">
                  No violations found for this participant.
                </Typography>
              </Stack>
            </Paper>
          )}
          {violations?.map(
            (violation: Violation | null, index) =>
              violation && (
                <Paper>
                  <Stack key={index} direction="row" spacing={1} px={2} py={3}>
                    <Typography variant="subtitle1" color="text.main" fontWeight={600}>
                      {index + 1}.
                    </Typography>
                    <Stack
                      direction="column"
                      spacing={2}
                      mt={1}
                      flexWrap="wrap"
                    >
                      <Typography variant="subtitle1" color="text.main">
                        Violation type: {violation.violationType}
                      </Typography>
                      <Typography variant="body1" color="text.main">
                        Description: {violation.description}
                      </Typography>
                    </Stack>
                  </Stack>
                </Paper>
              )
          )}
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
    </>
  );
};

export default ParticipantViolationsPage;
