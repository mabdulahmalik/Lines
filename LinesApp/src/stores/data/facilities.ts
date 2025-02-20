import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  CreateFacilityCmd,
  Facility,
  useGetFacilitiesQuery,
  useBroadcastSubscription,
  useDirectSubscription,
  useCreateFacilityMutation,
  FacilityRoom,
  CreateFacilityRoomCmd,
  useCreateFacilityRoomMutation,
  useGetFacilityRoomsQuery,
  ModifyFacilityRoomCmd,
  useModifyFacilityRoomMutation,
  ModifyFacilityCmd,
  useModifyFacilityMutation,
  ArchiveFacilityCmd,
  useArchiveFacilityMutation,
  DeleteFacilityCmd,
  useDeleteFacilityMutation,
  SortFacilityCmd,
  useSortFacilityMutation,
  UnarchiveFacilityCmd,
  useUnarchiveFacilityMutation,
} from '@/api/__generated__/graphql';
import { useToast } from 'vue-toastification';
const toast = useToast();

export const useFacilitiesStore = defineStore('facilities', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);
  const isFacilitiesLoading = ref(true);
  const isRoomsLoading = ref(true);

  const facilities = ref<Facility[]>([]);
  const rooms = ref<FacilityRoom[]>([]);

  // Fetch Facilities
  const { onResult: getFacilities } = useGetFacilitiesQuery();
  getFacilities((result: any) => {
    facilities.value = result?.data?.facilities?.items || [];
    isFacilitiesLoading.value = result.loading;
  });

  const { onResult: onGetRooms } = useGetFacilityRoomsQuery();
  onGetRooms((result: any) => {
    rooms.value = result?.data?.facilityRooms?.items || [];
    isRoomsLoading.value = result.loading;
  });

  // GET Rooms
  const getRooms = (facilityId: string) => {
    const { onResult } = useGetFacilityRoomsQuery({ where: { facilityId: { eq: facilityId } } } as any);
    onResult((result: any) => {
      if (result?.data && result?.data?.facilityRooms) {
        rooms.value = result?.data?.facilityRooms?.items as FacilityRoom[];
      }
    });
    return onResult;
  };

  // GET Room Count For Facility
  const getRoomCountForFacility = async (id: string): Promise<number> => {
    return new Promise((resolve) => {
      const { onResult } = useGetFacilityRoomsQuery({ where: { facilityId: { eq: id } } } as any);
      onResult((result: any) => {
        if (result.data && result.data.facilityRooms) {
          resolve(result.data.facilityRooms.totalCount);
        }
      });
    });
  };

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) {
      return;
    }
    const payload = JSON.parse(result.data.broadcast.payload);

    if (
      result.data.broadcast.eventName === 'ObjectCreated' &&
      payload?.id &&
      payload?.name === 'Facility'
    ) {
      getFacility(payload.id)((result: any) => {
        if (result?.data?.facilities?.items?.length == 1) {
          facilities.value = [
            ...facilities.value.filter((f) => f.id !== payload.id),
            result.data.facilities.items[0],
          ];
        }
      });
    } else if (
      result.data.broadcast.eventName === 'ObjectCreated' &&
      payload?.id &&
      payload?.name === 'FacilityRoom'
    ) {
      getRoom(payload.id)((result: any) => {
        if (result?.data?.facilityRooms?.items?.length == 1) {
          rooms.value = [
            ...rooms.value.filter((f) => f.id !== payload.id),
            result.data.facilityRooms.items[0],
          ];
        }
      });
    } else if (
      result.data.broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload?.name === 'FacilityRoom'
    ) {
      getRoom(payload.id)((result: any) => {
        if (result?.data?.facilityRooms?.items?.length == 1) {
          rooms.value = [
            ...rooms.value.filter((r) => r.id !== payload.id),
            result.data.facilityRooms.items[0],
          ];
        }
      });
    } else if (
      result.data.broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload?.name === 'Facility'
    ) {
      getFacility(payload.id)((result: any) => {
        if (result?.data?.facilities?.items?.length == 1) {
          facilities.value = [
            ...facilities.value.filter((r) => r.id !== payload.id),
            result.data.facilities.items[0],
          ];
        }
      });
    } else if (
      result.data.broadcast.eventName === 'ObjectDeleted' &&
      payload?.ids &&
      payload?.name === 'Facility'
    ) {
      facilities.value = facilities.value.filter((f) => !payload.ids.includes(f.id));
      if (selectedFacility.value?.id && payload.ids.includes(selectedFacility.value.id)) {
        clearSelectedFacility();
      }
    } else if (
      result.data.broadcast.eventName === 'ObjectArchiveStateChanged' &&
      payload?.ids &&
      payload?.name === 'Facility'
    ) {
      facilities.value = facilities.value.map((f) => {
        if (payload.ids.includes(f.id)) {
          return {
            ...f,
            archived: !f.archived,
          };
        }
        return f;
      });
      if (selectedFacility.value?.id === payload.ids[0]) {
        selectedFacility.value = {
          ...selectedFacility.value,
          archived: payload.archived,
        };
      }
    }
     // Sorted Facility Types
     else if (
      result.data.broadcast.eventName === 'ObjectSorted' &&
      payload?.id &&
      payload?.name === 'Facility'
    ) {
      // console.log('payload sorted', payload);
      // Facilities store updated
      const facilitiesCopy = [...facilities.value];
      const indexToMove = facilitiesCopy.findIndex((facility) => facility.id === payload.id);
      const zeroBasedPosition = payload.position - 1;
      if (
        indexToMove !== -1 &&
        zeroBasedPosition >= 0 &&
        zeroBasedPosition < facilitiesCopy.length
      ) {
        const [facilityToMove] = facilitiesCopy.splice(indexToMove, 1);
        facilitiesCopy.splice(zeroBasedPosition, 0, facilityToMove);
      }
      facilities.value = facilitiesCopy;
      // console.log(facilities.value);
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

  const getFacility = (id: string) => {
    const { onResult } = useGetFacilitiesQuery({ where: { id: { eq: id } } } as any);
    return onResult;
  };

  const getRoom = (id: string) => {
    const { onResult } = useGetFacilityRoomsQuery({ where: { id: { eq: id } } } as any);
    return onResult;
  };

  // Mutations
  const createFacility = async (payload: CreateFacilityCmd) => {
    const { mutate } = useCreateFacilityMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.createFacility?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  // create Room
  const createRoom = async (payload: CreateFacilityRoomCmd) => {
    const { mutate } = useCreateFacilityRoomMutation();
    var result = await mutate({ command: payload });
    const correlationId = result?.data?.createFacilityRoom?.correlationId ?? null;
    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  // modify room

  const modifyRoom = async (payload: ModifyFacilityRoomCmd) => {
    const { mutate } = useModifyFacilityRoomMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyFacilityRoom?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  // modify Facility

  const modifyFacility = async (payload: ModifyFacilityCmd) => {
    const { mutate } = useModifyFacilityMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyFacility?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  // archive Facility
  const archiveFacility = async (payload: ArchiveFacilityCmd) => {
    const { mutate } = useArchiveFacilityMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.archiveFacility?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const unarchiveFacility = async (payload: UnarchiveFacilityCmd) => {
    const { mutate } = useUnarchiveFacilityMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.unarchiveFacility?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deleteFacility = async (payload: DeleteFacilityCmd) => {
    const { mutate } = useDeleteFacilityMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deleteFacility?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

   // sorted Facilities
   const sortFacilities = async (payload: SortFacilityCmd) => {
    const { mutate } = useSortFacilityMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.sortFacility?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedFacility = ref<Facility>({
    createdAt: '',
    id: null,
    modifiedAt: '',
    archived: null,
    name: '',
    procedureIds: [],
    purposeIds: [],
    timeZone: '',
    typeId: '',
    address: null,
    requiredData: null,
    routineAssignments: [],
    referenceCount: 0,
  });

  function updatePurposeIds(purposeIds: string[]) {
    selectedFacility.value = {
      ...selectedFacility.value,
      purposeIds: purposeIds,
    };
  }

  function updateProcedureIds(procedureIds: string[]) {
    selectedFacility.value = {
      ...selectedFacility.value,
      procedureIds: procedureIds,
    };
  }

  function updateRoutineAssignments(routineAssignments: any[]) {
    selectedFacility.value = {
      ...selectedFacility.value,
      routineAssignments: routineAssignments,
    };
  }

  function clearSelectedFacility() {
    selectedFacility.value = {
      createdAt: '',
      id: null,
      modifiedAt: '',
      archived: null,
      name: '',
      procedureIds: [],
      purposeIds: [],
      timeZone: '',
      typeId: '',
      address: null,
      requiredData: null,
      routineAssignments: [],
      referenceCount: 0,
    };
  }

  return {
    facilities,
    isFacilitiesLoading,
    isRoomsLoading,
    createFacility,
    selectedFacility,
    clearSelectedFacility,
    rooms,
    getRooms,
    createRoom,
    modifyRoom,
    modifyFacility,
    getRoomCountForFacility,
    updateProcedureIds,
    updatePurposeIds,
    updateRoutineAssignments,
    archiveFacility,
    unarchiveFacility,
    deleteFacility,
    sortFacilities,
    getFacility,
    getRoom
  };
});
