<script setup lang="ts">
import { ref, computed, toRaw, onMounted, watch } from 'vue';
import { FwbButton, FwbSelect, FwbInput, FwbRadio } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { useFacilitiesStore } from '@/stores/data/facilities';
import {
  RoutineAssignmentSpecPrm,
  RoutineAssignmentPrm,
  Facility,
} from '@/api/__generated__/graphql';

const props = defineProps<{
  exec: RoutineAssignmentPrm;
  facility?: Facility;
}>();

const emit = defineEmits(['close', 'updatedExec']);

const routinesStore = useRoutinesStore();
const facilityTypesStore = useFacilityTypesStore();
const facilitiesStore = useFacilitiesStore();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const facilityTypes = computed(() => facilityTypesStore.facilityTypes);
const roomProperties = computed(() => {
  const facilityType = facilityTypes.value.find(
    (facility) => facility.id === facilitiesStore.selectedFacility.typeId
  );
  return facilityType ? facilityType.properties : [];
});

const routines = computed(() => routinesStore.routines);

const routineOptions = computed(() =>
  routines.value.map((r) => ({
    value: r.id,
    name: r.name!,
  }))
);

const editExecutionModalRef = ref<InstanceType<typeof ModalDrawer>>();

onMounted(() => {
  executionName.value = props.exec?.name;
});

watch(
  () => props.exec,
  (newValue) => {
    console.log('new vals', newValue);
    executionName.value = newValue.name;
    routineSeletion.value = newValue.routineId;

    if (newValue.specs && newValue.specs.length > 0) {
      const validSpecs = newValue.specs.filter(
        (spec): spec is RoutineAssignmentSpecPrm => spec !== null && spec !== undefined
      );
      updatePropertiesFromSpecs(propertiesForAssignment.value, validSpecs);
      assignedTo.value = 'SpecificAreas';
      // console.log('hdjhd')
    }
  }
);

// Function to update properties based on specs
function updatePropertiesFromSpecs(
  propertiesForAssignment: RoutineAssignmentSpecPrm[],
  specs: RoutineAssignmentSpecPrm[]
) {
  specs.forEach((spec) => {
    const matchingProperty = propertiesForAssignment.find(
      (property) => property.propertyId === spec.propertyId
    );

    if (matchingProperty) {
      matchingProperty.propertyValue = spec.propertyValue;
    }
  });
}

function closeAddExecutionModal() {
  editExecutionModalRef.value?.setModalOpen(false);
}
function AddExecutionModalClosed() {
  emit('close');
}
const assignedTo = ref('EntireFacility');
function getRoomOptions(room: any) {
  return room.options.map((o: any) => ({
    value: o.id,
    name: o.text,
  }));
}
function resetPropertiesOnToggle(val: string) {
  if (val === 'EntireFacility') {
    propertiesForAssignment.value = [];
  }
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
  // console.log('p f a', propertiesForAssignment.value);
}

// Validate Add Execution Form

const schemaAddExecution = yup.object({
  executionName: yup.string().required('This is a required field'),
  routineSeletion: yup.string().required('This is a required field'),
});

const { handleSubmit: handleSubmitEditExecution, errors: addExecutionErrors } = useForm({
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
function handleEditExecution() {
  handleSubmitEditExecution(async () => {
    const clonedProperties = JSON.parse(JSON.stringify(toRaw(propertiesForAssignment.value)));
    const updatedExecution = {
      id: props.exec.id,
      name: executionName.value,
      routineId: routineSeletion.value,
      specs: clonedProperties.filter(
        (property: RoutineAssignmentSpecPrm) => property.propertyId && property.propertyValue
      ),
    };
    emit('updatedExec', updatedExecution);
    console.log('Updated Executiion', updatedExecution);
    closeAddExecutionModal();
    resetPropertyValues();
    resetExecutionName();
    resetRoutineSeletion();
  })();
}

const setModalOpen = (value: boolean): void => {
  editExecutionModalRef.value?.setModalOpen(value);
};

defineExpose({
  setModalOpen,
});
</script>

<template>
  <ModalDrawer
    ref="editExecutionModalRef"
    max_width="xl"
    set_margins
    @close="AddExecutionModalClosed"
  >
    <template #header>
      <h3 class="text-base lg:text-xl font-semibold text-gray-900">Edit Assignment</h3>
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
              facility?.archived ? 'pointer-events-none' : '',
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
            <fwb-radio
              v-model="assignedTo"
              label="Entire Facility"
              value="EntireFacility"
              @update:model-value="resetPropertiesOnToggle"
            />
          </div>
          <div>
            <fwb-radio v-model="assignedTo" label="Specific Areas" value="SpecificAreas" />
          </div>
        </div>
        <!-- if Specific Areas -->
        <div v-if="assignedTo === 'SpecificAreas'" class="flex flex-col gap-6">
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
            @click.prevent="handleEditExecution"
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
