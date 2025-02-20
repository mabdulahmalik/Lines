import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  DeleteMedicalRecordCmd,
  MedicalRecord,
  ModifyMedicalRecordCmd,
  RenumberMedicalRecordCmd,
  useBroadcastSubscription,
  useDeleteMedicalRecordMutation,
  useDirectSubscription,
  useGetMedicalRecordsQuery,
  useModifyMedicalRecordMutation,
  useRenumberMedicalRecordMutation,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';

const toast = useToast();

export const useMedicalRecordsStore = defineStore('medicalRecords', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isLoading = ref(true);

  const medicalRecords = ref<MedicalRecord[]>([]);

  const { onResult } = useGetMedicalRecordsQuery();

  onResult((result: any) => {
    medicalRecords.value = result?.data?.medicalRecords?.items || [];
    isLoading.value = result.loading;
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();
  handleBroadcast((result) => {
    //  console.log( 'result', result);
    if (!result.data?.broadcast?.payload) {
      return;
    }
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (
      broadcast.eventName === 'ObjectCreated' &&
      payload?.id &&
      payload.name === 'MedicalRecord'
    ) {
      getMedicalRecord(payload.id)((result: any) => {
        if (result?.data?.medicalRecords?.items?.length == 1) {
          medicalRecords.value = [
            ...medicalRecords.value.filter((l) => l.id !== payload.id),
            result.data.medicalRecords.items[0],
          ];
        }
      });
    }
    else if (broadcast.eventName === 'ObjectModified' && payload?.id && payload.name === 'MedicalRecord') {
      getMedicalRecord(payload.id)((result: any) => {
        if (result?.data?.medicalRecords?.items?.length == 1) {
          medicalRecords.value = [
            ...medicalRecords.value.filter((l) => l.id !== payload.id),
            result.data.medicalRecords.items[0],
          ];
          if (selectedMedicalRecord.value?.id === payload.id) {
            selectedMedicalRecord.value = result.data?.medicalRecords?.items[0];
          }
        }
      });
    }

    else if (broadcast.eventName === 'MedicalRecordObservationMessage' && payload?.id) {
      getMedicalRecord(payload.id)((result: any) => {
        if (result?.data?.medicalRecords?.items?.length == 1) {
          medicalRecords.value = [
            ...medicalRecords.value.filter((l) => l.id !== payload.id),
            result.data.medicalRecords.items[0],
          ];
        }
      });
    }
    else if (broadcast.eventName === 'MedicalRecordObservationsRemoved' && payload?.id) {
      getMedicalRecord(payload.id)((result: any) => {
        if (result?.data?.medicalRecords?.items?.length == 1) {
          medicalRecords.value = [
            ...medicalRecords.value.filter((l) => l.id !== payload.id),
            result.data.medicalRecords.items[0],
          ];
        }
      });
    }
    else if (broadcast.eventName === 'ObjectDeleted' && payload?.ids && payload?.name === 'MedicalRecord') {
      medicalRecords.value = medicalRecords.value.filter((mr) => !payload.ids.includes(mr.id));
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

  const getMedicalRecord = (id: string) => {
    const { onResult } = useGetMedicalRecordsQuery({ where: { id: { eq: id } } } as any, { fetchPolicy: 'network-only' });
    return onResult;
  };

  // Mutations
  const modifyMedicalRecord = async (payload: ModifyMedicalRecordCmd) => {
    const { mutate } = useModifyMedicalRecordMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyMedicalRecord?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const renumberMedicalRecord = async (payload: RenumberMedicalRecordCmd) => {
    const { mutate } = useRenumberMedicalRecordMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.renumberMedicalRecord?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deleteMedicalRecord = async (payload: DeleteMedicalRecordCmd) => {
    const { mutate } = useDeleteMedicalRecordMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deleteMedicalRecord?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedMedicalRecord = ref<MedicalRecord>({
    id: null,
    active: null,
    birthday: null,
    createdAt: null,
    firstName: null,
    lastName: null,
    lastSeenOn: null,
    linesIn: null,
    linesTotal: null,
    modifiedAt: null,
    number: null,
    facilityId: null,
    firstSeenOn: null,
    observations: null,
  });
  function clearSelectedMedicalRecord() {
    selectedMedicalRecord.value = {
      id: null,
      active: null,
      birthday: null,
      createdAt: null,
      firstName: null,
      lastName: null,
      lastSeenOn: null,
      linesIn: null,
      linesTotal: null,
      modifiedAt: null,
      number: null,
      facilityId: null,
      firstSeenOn: null,
      observations: null,
    };
  }

  return {
    medicalRecords,
    selectedMedicalRecord,
    clearSelectedMedicalRecord,
    getMedicalRecord,
    modifyMedicalRecord,
    deleteMedicalRecord,
    renumberMedicalRecord,
    isLoading,
  };
});
