<script setup lang="ts">
import { ref, computed, toRaw } from 'vue';
import { FwbButton, FwbSelect, FwbInput, FwbRadio } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import { RoutineAssignmentSpecPrm, Facility } from '@/api/__generated__/graphql';

const routinesStore = useRoutinesStore();
const facilityTypesStore = useFacilityTypesStore();
const facilitiesStore = useFacilitiesStore();
const props = defineProps<{
  facility?: Facility;
}>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const emit = defineEmits(['close', 'newExec']);

const facilityTypes = computed(() => facilityTypesStore.facilityTypes);
const roomProperties = computed(() => {
  const facilityType = facilityTypes.value.find(
    (facility) => facility.id === facilitiesStore.selectedFacility.typeId
  );
  return facilityType ? facilityType.properties : [];
});

const routines = computed(() => routinesStore.routines);

const routineOptions = computed(() =>
  routines.value
    .filter((r) => r.active)
    .map((r) => ({
      value: r.id,
      name: r.name!,
    }))
);

const addExecutionModalRef = ref<InstanceType<typeof ModalDrawer>>();
function closeAddExecutionModal() {
  addExecutionModalRef.value?.setModalOpen(false);
}

function AddExecutionModalClosed() {
  emit('close');
}

const assignedTo = ref('Entire Facility');

function getRoomOptions(room: any) {
  return room.options.map((o: any) => ({
    value: o.id, //
    name: o.text, //
  }));
}

const propertiesForAssignment = ref(
  roomProperties.value?.map((property) => ({
    propertyId: property?.id,
    propertyValue: '',
  })) || []
);

function resetPropertyValues() {
  propertiesForAssignment.value.forEach((property) => {
    property.propertyValue = '';
  });
}
function setPropertyValueOnSelect(index: number, val: string) {
  if (propertiesForAssignment.value && propertiesForAssignment.value[index]) {
    propertiesForAssignment.value[index] = {
      ...propertiesForAssignment.value[index],
      propertyValue: val,
    };
  }
  console.log('p f a', propertiesForAssignment.value);
}

// Validate Add Execution Form

const schemaAddExecution = yup.object({
  executionName: yup.string().required('This is a required field'),
  routineSeletion: yup.string().required('This is a required field'),
});

const { handleSubmit: handleSubmitAddExecution, errors: addExecutionErrors } = useForm({
  validationSchema: schemaAddExecution,
});
const {
  value: executionName,
  errorMessage: executionNameError,
  resetField: resetExecutionName,
} = useField<string>('executionName');
const {
  value: routineSeletion,
  errorMessage: routineSeletionError,
  resetField: resetRoutineSeletion,
} = useField<string>('routineSeletion');

const routineDescription = ref('');

const setRoutineDescription = computed(() => {
  const routine = routines.value.find((r) => r.id === routineSeletion.value);
  routineDescription.value = routine?.description ?? '';
});
function handleAddExecution() {
  handleSubmitAddExecution(async () => {
    const clonedProperties = JSON.parse(JSON.stringify(toRaw(propertiesForAssignment.value)));
    const newAddExecution = {
      id: null,
      name: executionName.value,
      routineId: routineSeletion.value,
      specs: clonedProperties.filter(
        (property: RoutineAssignmentSpecPrm) => property.propertyId && property.propertyValue
      ),
    };

    emit('newExec', newAddExecution);
    console.log('Add Execution Data', newAddExecution);
    closeAddExecutionModal();
    resetPropertyValues();
    resetExecutionName();
    resetRoutineSeletion();
  })();
}

const setModalOpen = (value: boolean): void => {
  addExecutionModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <ModalDrawer
    ref="addExecutionModalRef"
    max_width="xl"
    set_margins
    @close="AddExecutionModalClosed"
  >
    <template #header>
      <h3 class="text-base lg:text-xl font-semibold text-gray-900">Add New Assignment</h3>
    </template>
    <template #body>
      <!-- body -->
      <div class="flex flex-col gap-6">
        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Name</label>
          <fwb-input
            v-model="executionName"
            placeholder="specify Assignment Name here ..."
            :class="[
              executionNameError ? inputErrorClasses : '',
              props.facility?.archived ? 'pointer-events-none' : '',
            ]"
          />
          <span v-if="executionNameError" class="text-sm text-radical-red-600">{{
            executionNameError
          }}</span>
        </div>

        <div>
          <label class="mb-1 block text-sm font-medium text-slate-900">Routine Selection</label>
          <fwb-select
            v-model="routineSeletion"
            :options="routineOptions"
            @change="setRoutineDescription"
            placeholder="Select"
            :class="[
              routineSeletionError ? inputErrorClasses : '',
              facility?.archived ? 'pointer-events-none' : '',
            ]"
          />
          <span v-if="routineSeletionError" class="text-sm text-radical-red-600">{{
            routineSeletionError
          }}</span>
        </div>
        <!-- show if Routine Selection is select -->
        <div
          v-if="routineSeletion && routineDescription"
          class="bg-brand-50 rounded-lg p-4 text-sm font-normal"
        >
          {{ routineDescription }}
        </div>

        <hr class="border-slate-300" />
        <div class="font-semibold text-xl">Assigned to</div>
        <div class="flex justify-start items-center gap-6">
          <div>
            <fwb-radio v-model="assignedTo" label="Entire Facility" value="Entire Facility" />
          </div>
          <div>
            <fwb-radio v-model="assignedTo" label="Specific Areas" value="Specific Areas" />
          </div>
        </div>
        <!-- if Specific Areas -->
        <div v-if="assignedTo === 'Specific Areas'" class="flex flex-col gap-6">
          <div v-for="(room, indx) in roomProperties" :key="indx">
            <label class="mb-1 block text-sm font-medium text-slate-900">{{ room?.name }}</label>
            <fwb-select
              :model-value="propertiesForAssignment?.[indx]?.propertyValue"
              @update:model-value="setPropertyValueOnSelect(indx, $event)"
              :options="getRoomOptions(room)"
              placeholder="Select"
              :class="facility?.archived ? 'pointer-events-none' : ''"
            />
          </div>
        </div>
      </div>
    </template>
    <template #footer>
      <div class="flex items-center justify-end w-full">
        <div class="flex gap-6 sm:w-auto w-full items-center">
          <span
            v-if="Object.keys(addExecutionErrors).length"
            class="text-sm font-normal text-radical-red-600 text-nowrap"
            >{{ Object.keys(addExecutionErrors).length }} errors</span
          >
          <fwb-button @click="closeAddExecutionModal" color="light" pill> Cancel</fwb-button>
          <fwb-button
            @click.prevent="handleAddExecution"
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
