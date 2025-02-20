import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import {
  ActivateFacilityTypeCmd,
  CreateFacilityRoomPropertyCmd,
  CreateFacilityTypeCmd,
  DeactivateFacilityTypeCmd,
  FacilityType,
  FacilityRoomProperty,
  ModifyFacilityRoomPropertyCmd,
  RenameFacilityTypeCmd,
  useActivateFacilityTypeMutation,
  useAddFacilityRoomPropertyMutation,
  useBroadcastSubscription,
  useCreateFacilityTypeMutation,
  useDeactivateFacilityTypeMutation,
  useDirectSubscription,
  useGetFacilityTypesQuery,
  useModifyFacilityRoomPropertyMutation,
  useRenameFacilityTypeMutation,
  SortFacilityRoomPropertyCmd,
  useSortFacilityRoomPropertyMutation,
  SortFacilityTypeCmd,
  useSortFacilityTypeMutation,
} from '@/api/__generated__/graphql';

import { useToast } from 'vue-toastification';

const toast = useToast();

export const useFacilityTypesStore = defineStore('facilityTypes', () => {
  provideApolloClient(client);

  const currentCorrelationId = ref<string | null>(null);

  const facilityTypes = ref<FacilityType[]>([]);
  const roomProperties = ref<FacilityRoomProperty[]>([]);

  const { onResult } = useGetFacilityTypesQuery();

  onResult((result: any) => {
    facilityTypes.value = result?.data?.facilityTypes?.items || [];
    setRoomProperties();
  });

  const { onResult: handleBroadcast } = useBroadcastSubscription();

  handleBroadcast((result) => {
    if (!result.data?.broadcast?.payload) return;
    const broadcast = result.data.broadcast;
    const payload = JSON.parse(result.data.broadcast.payload);
    if (broadcast.eventName === 'ObjectCreated' && payload?.id && payload?.name == 'FacilityType') {
      getFacilityType(payload.id)((result: any) => {
        if (result?.data?.facilityTypes?.items?.length == 1) {
          facilityTypes.value = [
            ...facilityTypes.value.filter((ft) => ft.id !== payload.id),
            result.data.facilityTypes.items[0],
          ];
        }
      });
    } else if (
      broadcast.eventName === 'ObjectModified' &&
      payload?.id &&
      payload?.name == 'FacilityType'
    ) {
      getFacilityType(payload.id)((result: any) => {
        if (result?.data?.facilityTypes?.items?.length == 1) {
          facilityTypes.value = [
            ...facilityTypes.value.filter((ft) => ft.id !== payload.id),
            result.data.facilityTypes.items[0],
          ];
          setRoomProperties();
        }
      });
    }
    if (broadcast.eventName === 'FacilityRoomPropertyAdded' && payload?.id) {
      facilityTypes.value = facilityTypes.value.map((ft) =>
        ft.id === payload.facilityTypeId
          ? {
              ...ft,
              properties: [
                ...(ft.properties || []),
                { id: payload.id, name: payload.name, options: payload.options || [] },
              ],
            }
          : ft
      );
      setRoomProperties();
    } else if (broadcast.eventName === 'FacilityRoomPropertyModified' && payload?.id) {
      facilityTypes.value = facilityTypes.value.map((ft) =>
        ft.id === payload.facilityTypeId
          ? {
              ...ft,
              properties: (ft.properties || []).map((property) =>
                property?.id === payload.id
                  ? {
                      ...property,
                      id: property?.id,
                      name: payload.name,
                      options: payload.options || [],
                    }
                  : property
              ),
            }
          : ft
      );
      setRoomProperties();
    }
    // Sorted Facility Types
    else if (
      broadcast.eventName === 'ObjectSorted' &&
      payload?.id &&
      payload?.name == 'FacilityType'
    ) {
      // console.log('payload sorted', payload);
      // FacilityTypes store updated
      const facilityTypesCopy = [...facilityTypes.value];
      const indexToMove = facilityTypesCopy.findIndex((facility) => facility.id === payload.id);
      const zeroBasedPosition = payload.position - 1;
      if (
        indexToMove !== -1 &&
        zeroBasedPosition >= 0 &&
        zeroBasedPosition < facilityTypesCopy.length
      ) {
        const [facilityToMove] = facilityTypesCopy.splice(indexToMove, 1);
        facilityTypesCopy.splice(zeroBasedPosition, 0, facilityToMove);
      }
      facilityTypes.value = facilityTypesCopy;
      // console.log(facilityTypes.value);
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

  const getFacilityType = (id: any) => {
    const { onResult } = useGetFacilityTypesQuery({ where: { id: { eq: id } } } as any, {
      fetchPolicy: 'network-only',
    });
    return onResult;
  };

  const setRoomProperties = () => {
    roomProperties.value =
      facilityTypes.value?.flatMap((x: any) =>
        x.properties.map((p: any) => ({ ...p, facilityType: x.name, facilityTypeId: x.id }))
      ) || [];
  };

  // Mutations
  const createFacilityType = async (payload: CreateFacilityTypeCmd) => {
    const { mutate } = useCreateFacilityTypeMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.createFacilityType?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const renameFacilityType = async (payload: RenameFacilityTypeCmd) => {
    const { mutate } = useRenameFacilityTypeMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.renameFacilityType?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const activateFacilityType = async (payload: ActivateFacilityTypeCmd) => {
    const { mutate } = useActivateFacilityTypeMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.activateFacilityType?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const deactivateFacilityType = async (payload: DeactivateFacilityTypeCmd) => {
    const { mutate } = useDeactivateFacilityTypeMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.deactivateFacilityType?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const addRoomProperty = async (payload: CreateFacilityRoomPropertyCmd) => {
    const { mutate } = useAddFacilityRoomPropertyMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.addFacilityRoomProperty?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const modifyRoomProperty = async (payload: ModifyFacilityRoomPropertyCmd) => {
    const { mutate } = useModifyFacilityRoomPropertyMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.modifyFacilityRoomProperty?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };
  
  const sortRoomProperty = async (payload: SortFacilityRoomPropertyCmd) => {
    const { mutate } = useSortFacilityRoomPropertyMutation();
    await mutate({ command: payload });
  }
  
  // sorted Facility Types
  const sortFacilityType = async (payload: SortFacilityTypeCmd) => {
    const { mutate } = useSortFacilityTypeMutation();

    var result = await mutate({ command: payload });
    const correlationId = result?.data?.sortFacilityType?.correlationId ?? null;

    if (correlationId) {
      console.log('correlationId', correlationId);
      currentCorrelationId.value = correlationId;
    }
  };

  const selectedFacilityType = ref<FacilityType>({
    id: '',
    name: '',
    isActive: false,
    createdAt: '',
    modifiedAt: '',
    properties: [],
  });

  function clearSelectedFacilityType() {
    selectedFacilityType.value = {
      id: '',
      name: '',
      isActive: false,
      createdAt: '',
      modifiedAt: '',
      properties: [],
    };
  }
  const selectedRoomProperty = ref<FacilityRoomProperty>({
    id: '',
    name: '',
    options: [],
  });

  function clearSelectedRoomProperty() {
    selectedRoomProperty.value = {
      id: '',
      name: '',
      options: [],
    };
  }

  return {
    facilityTypes,
    roomProperties,
    createFacilityType,
    renameFacilityType,
    activateFacilityType,
    deactivateFacilityType,
    addRoomProperty,
    modifyRoomProperty,
    selectedFacilityType,
    clearSelectedFacilityType,
    selectedRoomProperty,
    clearSelectedRoomProperty,
    sortRoomProperty,
    sortFacilityType
  };
});
