<script setup lang="ts">
import { ref, watch, onMounted, toRaw, computed } from 'vue';
import { FwbButton, FwbInput } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import {
  FacilityRoomPropertyOption,
  FacilityRoom,
  ModifyFacilityRoomCmd,
  FacilityRoomPropertyPrm,
  Facility,
} from '@/api/__generated__/graphql';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.css';
import IconChevronDown from '@/components/icons/IconChevronDown.vue';

const facilityTypesStore = useFacilityTypesStore();
const facilitiesStore = useFacilitiesStore();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const props = defineProps<{
  room: FacilityRoom;
  facility?: Facility;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
}>();

// facility types
const facilityTypes = computed(() => facilityTypesStore.facilityTypes);
// rooms properties
const properties = computed(() => {
  const facilityType = facilityTypes.value.find(
    (facility) => facility.id === facilitiesStore.selectedFacility.typeId
  );
  return facilityType ? facilityType.properties : [];
});

const propertiesForRoom = ref(
  properties.value?.map((property) => ({
    optionId: '',
    propertyId: property?.id,
  })) || []
);

const editRoomModalRef = ref<InstanceType<typeof ModalDrawer>>();
function closeEditRoomModal() {
  editRoomModalRef.value?.setModalOpen(false);
}
function EditRoomModalClosed() {
  emit('close');
}
const setModalOpen = (value: boolean): void => {
  editRoomModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});

// Validation
const currentRoomName = ref(props.room.name || '');
console.log('currentRoomName', currentRoomName.value);
const existingRoomNames = computed(() => {
  return facilitiesStore.rooms.map((room) => room.name?.toLowerCase());
});

const schemaEditRoom = (currentRoomName: string) =>
  yup.object({
    editRoomName: yup
      .string()
      .required('This is a required field')
      .trim()
      .test('unique-name', 'Room name must be unique', (value) => {
        if (!value) return true;
        const filteredRoomNames = existingRoomNames.value.filter(
          (name) => name !== currentRoomName.toLowerCase()
        );
        return !filteredRoomNames.includes(value.toLowerCase());
      }),
  });

const { handleSubmit: handleSubmitEditRoom, errors: editRoomErrors } = useForm({
  validationSchema: computed(() => schemaEditRoom(currentRoomName.value)),
  validateOnMount: false,
});

const {
  value: editRoomName,
  errorMessage: editRoomNameError,
  resetField: resetEditRoomName,
} = useField<string>('editRoomName');

watch(
  () => props.room.name,
  (newValue) => {
    currentRoomName.value = newValue || '';
    editRoomName.value = newValue || '';
  },
  { immediate: true }
);

function handleEditRoom() {
  handleSubmitEditRoom(async () => {
    const clonedProperties = JSON.parse(JSON.stringify(toRaw(propertiesForRoom.value)));
    const newEditRoom: ModifyFacilityRoomCmd = {
      id: props.room.id,
      name: editRoomName.value,
      properties: clonedProperties.filter(
        (property: FacilityRoomPropertyPrm) => property && property.optionId
      ),
    };
    facilitiesStore.modifyRoom(newEditRoom);
    console.log('Edit Room Data', newEditRoom);
    closeEditRoomModal();
    resetPropertyValues();
    resetEditRoomName();
  })();
}

watch(
  () => props.room,
  (newValue) => {
    editRoomName.value = newValue.name!;
    if (newValue.properties) {
      initializeProperties();
    }
  },
  { immediate: true }
);

function initializeProperties() {
  propertiesForRoom.value = (properties.value || []).map((property: any) => {
    const roomProperty = props.room?.properties?.find(
      (roomProp: any) => roomProp.propertyId === property.id
    );
    return {
      optionId: roomProperty?.value || '',
      propertyId: property?.id,
    };
  });
}

function resetPropertyValues() {
  propertiesForRoom.value.forEach((property) => {
    property.optionId = '';
  });
}

function setPropertyValueOnSelect(index: number, val: any) {
  if (propertiesForRoom.value && propertiesForRoom.value[index]) {
    const selectedValue = {
      ...propertiesForRoom.value[index],
      optionId: val.value,
    };
    if (propertiesForRoom.value[index].optionId === val.value) {
      selectedValue.optionId = '';
    }
    propertiesForRoom.value[index] = selectedValue;
  }
  console.log('Properties for Rooms', propertiesForRoom.value);
}

// Function to extract options for each property
const getPropertyOptions = (property: any) => {
  return (property.options || []).map((option: FacilityRoomPropertyOption) => ({
    value: option.id,
    name: option.text,
  }));
};

const getPropertyOptionLabel = (property: any, id: string) => {
  return (property.options || []).find((option: FacilityRoomPropertyOption) => option.id === id)?.text;
};

watch(
  () => facilityTypesStore.roomProperties,
  () => {
    initializeProperties();
  }
);

onMounted(() => {
  initializeProperties();
  editRoomName.value = props.room.name!;
});
</script>

<template>
  <ModalDrawer ref="editRoomModalRef" max_width="xl" set_margins @close="EditRoomModalClosed">
    <template #header>
      <h3 class="text-base lg:text-xl font-semibold text-gray-900">Edit Room</h3>
    </template>
    <template #body>
      <!-- body -->
      <div class="flex flex-col gap-6">
        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Name</label>
          <fwb-input
            v-model="editRoomName"
            placeholder="Enter room name here ..."
            @blur="editRoomName = editRoomName?.trim()"
            :class="[
              editRoomNameError ? inputErrorClasses : '',
              facility?.archived ? 'pointer-events-none' : '',
            ]"
          />
          <span v-if="editRoomNameError" class="text-sm text-radical-red-600">{{
            editRoomNameError
          }}</span>
        </div>

        <div v-for="(property, index) in properties" :key="index">
          <div>
            <label class="mb-1 block text-sm font-medium text-slate-900">
              {{ property?.name }}
            </label>
            <multiselect
              :modelValue="propertiesForRoom?.[index]?.optionId"
              :options="getPropertyOptions(property)"
              @select="setPropertyValueOnSelect(index, $event)"
              track-by="value"
              label="name"
              placeholder="Select"
              :searchable="false"
              :allow-empty="true"
              selectLabel=""
              selectedLabel=""
              deselectLabel=""
              :class="['roomsSelectBox relative', facility?.archived ? 'pointer-events-none' : '']"
            >
              <template #singleLabel="{ option }">
                {{ getPropertyOptionLabel(property, option) }}
              </template>
              <template #caret>
                <span class="absolute right-2 top-1/2 transform -translate-y-1/2">
                  <IconChevronDown class="h-4 w-4" />
                </span>
              </template>
            </multiselect>
          </div>
        </div>
      </div>
    </template>
    <template #footer>
      <div class="flex items-center justify-end w-full">
        <div class="flex gap-6 sm:w-auto w-full items-center">
          <span
            v-if="Object.keys(editRoomErrors).length"
            class="text-sm font-normal text-radical-red-600 text-nowrap"
            >{{ Object.keys(editRoomErrors).length }} errors
          </span>
          <fwb-button @click="closeEditRoomModal" color="light" pill> Cancel</fwb-button>
          <fwb-button
            @click.prevent="handleEditRoom"
            class="bg-primary-600 hover:bg-brand-600 w-full"
            pill
            :disabled="facility?.archived ?? false"
          >
            Save
          </fwb-button>
        </div>
      </div>
    </template>
  </ModalDrawer>
</template>

<style>
/* customize vue muliselect box */
.roomsSelectBox .multiselect__tags {
  background-color: rgb(249 250 251);
  border-color: #cbd5e1;
  border-radius: 8px;
}
.roomsSelectBox .multiselect__tags:focus {
  border-color: #1c64f2;
}
.roomsSelectBox .multiselect__single {
  font-size: 14px !important;
}
.roomsSelectBox .multiselect__option--highlight {
  background-color: #1c64f2;
  color: #ffffff;
}
.roomsSelectBox .multiselect__placeholder {
  color: rgb(75 85 99) !important;
}
.roomsSelectBox .multiselect__content-wrapper {
  box-shadow: 0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1);
  border-radius: 8px;
}

.roomsSelectBox .multiselect__option {
  padding: 6px 10px;
  min-height: 18px;
  font-size: 0.8rem;
  font-weight: 500;
}
</style>
