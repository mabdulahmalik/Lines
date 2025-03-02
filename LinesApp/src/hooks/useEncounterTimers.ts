import { Encounter, EncounterAlertType, EncounterStage, Job, JobStatus } from '@/api/__generated__/graphql';
import DateTimeFormatter from '@/utils/dateTimeFormatter';
import { calculateDuration, calculateElapsedTime, formatMinutesToDuration } from '@/utils/dateUtils';
import { ref, watch, onMounted, onUnmounted, computed } from 'vue';

const INTERVAL_DURATION = 60000;

export function useEncounterTimers(props: { encounter: (Encounter | undefined), job: (Job | undefined) }) {

  const intervalId = ref<NodeJS.Timeout | null>(null);
  const relativeTimeJob = ref('');
  const relativeTimeEnc = ref('');
  const elapsedTime = ref('');
  const holdElapsedTime = ref('');
  const attendingDuration = ref('');

  // Function to update timers
  function updateTime(): void {
    if (!props.encounter?.createdAt && !props.job?.createdAt) return;

    updateElapsedTime();
    updateHoldElapsedTime();
    updateRelativeTime();
    updateAttendingDuration();
  }

  function updateElapsedTime(): void {
    if (props.encounter?.stage !== EncounterStage.Attending) {
      return;
    }
  
    const currentProgress = props.encounter?.progress?.find(
      pro => pro?.stage === EncounterStage.Attending
    );
  
    if (!currentProgress?.enteredAt) {
      return;
    }
  
    elapsedTime.value = props.job?.status === JobStatus.Canceled
      ? calculateDuration(currentProgress.enteredAt, props.job?.statusChangedAt)
      : calculateElapsedTime(currentProgress.enteredAt);
  }  

  function updateHoldElapsedTime(): void {
    const alertedAt = props.encounter?.alerts?.find(alert => alert?.type === EncounterAlertType.OnHold)?.alertedAt;
    holdElapsedTime.value = calculateElapsedTime(alertedAt);
  }

  function updateRelativeTime(): void {
    relativeTimeJob.value = DateTimeFormatter.formatDatetime(props.encounter?.createdAt);
    relativeTimeEnc.value = DateTimeFormatter.formatDatetime(props.encounter?.createdAt);
  }

  function updateAttendingDuration(): void {
    attendingDuration.value = formatMinutesToDuration(
      props.encounter?.progress?.find((x) => x?.stage === EncounterStage.Attending)?.duration
    );
  }

  const encounterTime = computed(() => {
    if (!props.encounter?.progress) return '00:00';
    const relevantStages = props.encounter.progress.filter(
      (stage) => stage?.stage && [EncounterStage.Assigned, EncounterStage.Attending, EncounterStage.Charting].includes(stage.stage)
    );
    const totalMinutes = relevantStages.reduce((sum, stage) => {
      return sum + (stage?.duration ?? 0);
    }, 0);
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
  });

  // Watch for changes in encounter and job props
  watch(() => props.encounter, updateTime);
  watch(() => props.job, updateTime);

  // Set up the interval to periodically update the time
  onMounted(() => {
    updateTime();
    intervalId.value = setInterval(updateTime, INTERVAL_DURATION);
  });

  // Cleanup interval on unmount
  onUnmounted(() => {
    if (intervalId.value) clearInterval(intervalId.value);
  });

  return {
    relativeTimeJob,
    relativeTimeEnc,
    elapsedTime,
    holdElapsedTime,
    attendingDuration,
    encounterTime
  };
}
