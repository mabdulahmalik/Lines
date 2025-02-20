<script setup lang="ts">
import { ref, onMounted, computed, watch, toRaw } from 'vue';
import { FwbButton, FwbInput } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { Facility, FacilityRoomPropertyOption, CreateFacilityRoomCmd } from '@/api/__generated__/graphql';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import Multiselect from 'vue-multiselect';
import 'vue-multiselect/dist/vue-multiselect.css';
import IconChevronDown from '@/components/icons/IconChevronDown.vue';

const facilitiesStore = useFacilitiesStore();
const facilityTypesStore = useFacilityTypesStore();

const props = defineProps<{
  facility?: Facility;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
}>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
onMounted(() => {
  facilitiesStore.getRooms(facilitiesStore.selectedFacility.id);
});

// facility types
const facilityTypes = computed(() => facilityTypesStore.facilityTypes);
// rooms properties
const properties = computed(() => {
  const facilityType = facilityTypes.value.find(
    (facility) => facility.id === facilitiesStore.selectedFacility.typeId
  );
  return facilityType ? facilityType.properties : [];
});

const propertiesForRooms = ref(
  properties.value?.map((property) => ({
    propertyId: property?.id,
    optionId: '',
  })) || []
);
watch(
  () => facilityTypesStore.roomProperties,
  () => {
    propertiesForRooms.value =
      properties.value?.map((property: any) => ({
        propertyId: property?.id,
        optionId: '',
      })) || [];
  }
);

// Modal
const addRoomModalRef = ref<InstanceType<typeof ModalDrawer>>();
function closeAddRoomModal() {
  addRoomModalRef.value?.setModalOpen(false);
}
function AddRoomModalClosed() {
  emit('close');
}

const setModalOpen = (value: boolean): void => {
  addRoomModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});

interface RoomItem {
  optionId: string;
  propertyId: string;
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

function resetPropertyValues() {
  propertiesForRooms.value.forEach((property: any) => {
    property.optionId = '';
  });
}
function setPropertyValueOnSelect(index: number, val: any) {
  if (propertiesForRooms.value && propertiesForRooms.value[index]) {
    const selectedValue = {
      ...propertiesForRooms.value[index],
      optionId: val.value,
    };
    if (propertiesForRooms.value[index].optionId === val.value) {
      selectedValue.optionId = '';
    }
    propertiesForRooms.value[index] = selectedValue;
  }
  console.log('Properties for Rooms', propertiesForRooms.value);
}

// Validation
const existingRoomNames = computed(() => {
  return facilitiesStore.rooms.map((room) => room.name?.toLowerCase());
});

const schemaAddRoom = yup.object({
  name: yup
    .string()
    .required('This is a required field')
    .trim()
    .test('unique-name', 'Room name must be unique', (value) => {
      if (!value) return true;
      return !existingRoomNames.value.includes(value.toLowerCase());
    }),
});

const { handleSubmit: handleSubmitAddRoom, errors: addRoomErrors } = useForm({
  validationSchema: schemaAddRoom,
});
const { value: name, errorMessage: nameError, resetField: resetName } = useField<string>('name');

function handleAddRoom() {
  handleSubmitAddRoom(async () => {
    const clonedProperties = JSON.parse(JSON.stringify(toRaw(propertiesForRooms.value)));
    const addnewRoom: CreateFacilityRoomCmd = {
      facilityId: props.facility?.id,
      name: name.value,
      properties: clonedProperties.filter(
        (property: RoomItem) => property.propertyId && property.optionId
      ),
    };
    facilitiesStore.createRoom(addnewRoom);
    console.log('Add Room Data', addnewRoom);
    closeAddRoomModal();
    resetName();
    resetPropertyValues();
  })();
}
</script>

<template>
  <ModalDrawer ref="addRoomModalRef" max_width="xl" set_margins @close="AddRoomModalClosed">
    <template #header>
      <h3 class="text-base lg:text-xl font-semibold text-gray-900">Add Room</h3>
    </template>
    <template #body>
      <!-- body -->
      <div class="flex flex-col gap-6">
        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Name</label>
          <fwb-input
            v-model="name"
            placeholder="Enter room name here ..."
            @blur="name = name?.trim()"
            :class="[
              nameError ? inputErrorClasses : '',
              facility?.archived ? 'pointer-events-none' : '',
            ]"
          />
          <span v-if="nameError" class="text-sm text-radical-red-600">{{ nameError }}</span>
        </div>

        <div v-for="(property, indx) in properties" :key="indx">
          <div>
            <label class="mb-1 block text-sm font-medium text-slate-900">
              {{ property?.name }}
            </label>
            <multiselect
              :modelValue="propertiesForRooms?.[indx]?.optionId"
              :options="getPropertyOptions(property)"
              @select="setPropertyValueOnSelect(indx, $event)"
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
            v-if="Object.keys(addRoomErrors).length"
            class="text-sm font-normal text-radical-red-600 text-nowrap"
            >{{ Object.keys(addRoomErrors).length }} errors</span
          >
          <fwb-button @click="closeAddRoomModal" color="light" pill> Cancel</fwb-button>
          <fwb-button
            @click.prevent="handleAddRoom"
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
