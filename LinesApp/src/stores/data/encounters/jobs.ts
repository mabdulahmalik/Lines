import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  AttachNoteToJobCmd,
  EnactJobIntakePrc,
  Job,
  ModifyNoteForJobCmd,
  RemoveNoteFromJobCmd,
  useAttachNoteToJobMutation,
  useBroadcastSubscription,
  useEnactJobIntakeMutation,
  useDirectSubscription,
  useGetJobsQuery,
  useModifyNoteForJobMutation,
  useRemoveNoteFromJobMutation,
  CancelJobCmd,
  useCancelJobMutation,
  DeleteJobCmd,
  useDeleteJobMutation,
  PinNoteToJobCmd,
  usePinNoteToJobMutation,
  UnpinNoteFromJobCmd,
  useUnpinNoteFromJobMutation,
  MakeNoteAnObservationCmd,
  useMakeNoteAnObservationMutation,
  DiscardNoteAsObservationCmd,
  useDiscardNoteAsObservationMutation,
  useEnactJobRescheduleMutation,
  EnactJobReschedulePrc,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useJobsStore = defineStore('jobs', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const jobs = ref<Job[]>([]);

  const { onResult } = useGetJobsQuery();

  onResult((result: any) => {
    jobs.value = result?.data?.jobs?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    console.log('job result', result);
    if (!result.data?.broadcast?.payload) return;
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (broadcast.eventName === 'ObjectCreated' && payload?.id && payload.name === 'Job') {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
        }
      });
    } else if (broadcast.eventName === 'ObjectModified' && payload?.id && payload.name === 'Job') {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          if (selectedJob.value.id) {
            selectedJob.value = res.data.jobs.items[0];
          }
        }
      });
    } else if (broadcast.eventName === 'ObjectDeleted' && payload?.ids && payload.name === 'Job') {
      jobs.value = jobs.value.filter((j) => j.id !== payload.ids[0]);
      if (selectedJob.value.id) {
        clearSelectedJob();
      }
    } else if (broadcast.eventName === 'JobNotesAdded' && payload?.id) {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          selectedJob.value = {
            ...selectedJob.value,
            notes: res.data.jobs.items[0].notes,
          };
        }
      });
    } else if (broadcast.eventName === 'JobNotesModified' && payload?.id) {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          selectedJob.value = {
            ...selectedJob.value,
            notes: res.data?.jobs?.items[0].notes,
          };
        }
      });
    } else if (broadcast.eventName === 'JobNotesRemoved' && payload?.id) {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          selectedJob.value = {
            ...selectedJob.value,
            notes: res.data?.jobs?.items[0].notes,
          };
        }
      });
    } 
    else if (broadcast.eventName === 'JobStatusChanged' && payload?.id) {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          selectedJob.value = {
            ...selectedJob.value,
            status: res.data?.jobs?.items[0].status,
          };
        }
      });
    } else if (broadcast.eventName === 'JobRescheduled' && payload?.id) {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          if(selectedJob.value.id){
            selectedJob.value = res.data.jobs.items[0]
          }
        }
      });
    }
    else if (broadcast.eventName === 'MedicalRecordObservationMessage' && payload?.id) {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          selectedJob.value = {
            ...selectedJob.value,
            notes: res.data?.jobs?.items[0].notes,
          };
        }
      });
    }
    else if (broadcast.eventName === 'MedicalRecordObservationsRemoved' && payload?.id) {
      getJob(payload.id)((res: any) => {
        if (res?.data?.jobs?.items?.length == 1) {
          jobs.value = [...jobs.value.filter((j) => j.id !== payload.id), res.data.jobs.items[0]];
          selectedJob.value = {
            ...selectedJob.value,
            notes: res.data?.jobs?.items[0].notes,
          };
        }
      });
    }    
  });

  const { onResult: handleDirectSubscription } = useDirectSubscription();
  handleDirectSubscription((result) => {
    if (!result.data?.direct?.payload) return;
    const payload = JSON.parse(result.data.direct.payload);
    const correlationId = result.data.direct.correlationId;
    if (
      result.data.direct.eventName === 'OperationErrored' &&
      correlationId === currentCorrelationId.value
    ) {
      toast.error(`An error occurred at server.`);
      const errors = payload?.errors || [];
      if (errors.length > 0) {
        console.error(errors);
      }
    }
  });

  const getJob = (id: string) => {
    const { onResult } = useGetJobsQuery({ where: { id: { eq: id } } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  };

  // Mutations
  const createJob = async (payload: EnactJobIntakePrc) => {
    const { mutate } = useEnactJobIntakeMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.enactJobIntake?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const attachNoteToJob = async (payload: AttachNoteToJobCmd) => {
    const { mutate } = useAttachNoteToJobMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.attachNoteToJob?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const modifyNoteForJob = async (payload: ModifyNoteForJobCmd) => {
    const { mutate } = useModifyNoteForJobMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyNoteForJob?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  // Pinned Note
  const pinNoteToJob = async (payload: PinNoteToJobCmd) => {
    const { mutate } = usePinNoteToJobMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.pinNoteToJob?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };
    // UnPinned Note
    const unpinNoteFromJob = async (payload: UnpinNoteFromJobCmd) => {
      const { mutate } = useUnpinNoteFromJobMutation();
      var result = await mutate({ command: payload });
      const correlationId = result?.data?.unpinNoteFromJob?.correlationId ?? null;
      if (correlationId) {
        console.log('correlationId', correlationId);
        currentCorrelationId.value = correlationId;
      }
    };

    // Note observation
    const makeNoteAnObservation = async (payload: MakeNoteAnObservationCmd) => {
      const { mutate } = useMakeNoteAnObservationMutation();
      var result = await mutate({ command: payload });
      const correlationId = result?.data?.makeNoteAnObservation?.correlationId ?? null;
      if (correlationId) {
        console.log('correlationId', correlationId);
        currentCorrelationId.value = correlationId;
      }
    };

    const discardNoteAsObservation = async (payload: DiscardNoteAsObservationCmd) => {
      const { mutate } = useDiscardNoteAsObservationMutation();
      var result = await mutate({ command: payload });
      const correlationId = result?.data?.discardNoteAsObservation?.correlationId ?? null;
      if (correlationId) {
        console.log('correlationId', correlationId);
        currentCorrelationId.value = correlationId;
      }
    };

  const removeNoteFromJob = async (payload: RemoveNoteFromJobCmd) => {
    const { mutate } = useRemoveNoteFromJobMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.removeNoteFromJob?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const cancelJob = async (payload: CancelJobCmd) => {
    const { mutate } = useCancelJobMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.cancelJob?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deleteJob = async (payload: DeleteJobCmd) => {
    const { mutate } = useDeleteJobMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deleteJob?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const enactJobReschedule = async (payload: EnactJobReschedulePrc) => {
    const { mutate } = useEnactJobRescheduleMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.enactJobReschedule?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedJob = ref<Job>({
    statusChangedAt: null,
    contact: null,
    createdAt: null,
    id: null,
    modifiedAt: null,
    orderingProvider: null,
    purposeId: null,
    status: null,
    location: null,
    notes: null,
    lineId: null,
    medicalRecordId: null,
    origin: null,
    schedule: null,
  });
  function clearSelectedJob() {
    selectedJob.value = {
      statusChangedAt: null,
      contact: null,
      createdAt: null,
      id: null,
      modifiedAt: null,
      orderingProvider: null,
      purposeId: null,
      status: null,
      location: null,
      notes: null,
      lineId: null,
      medicalRecordId: null,
      origin: null,
      schedule: null,
    };
  }

  return {
    jobs,
    isLoading,
    selectedJob,
    clearSelectedJob,
    createJob,
    attachNoteToJob,
    modifyNoteForJob,
    removeNoteFromJob,
    cancelJob,
    deleteJob,
    pinNoteToJob,
    unpinNoteFromJob,
    makeNoteAnObservation,
    discardNoteAsObservation,
    enactJobReschedule,
  };
});
