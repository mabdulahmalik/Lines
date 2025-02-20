<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { FwbButton, FwbTooltip } from 'flowbite-vue';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import { useModalStore } from '@/stores/modal';
import AddExecutionModal from '../../modal/AddExecutionModal.vue';
import EditExecutionModal from '../../modal/EditExecutionModal.vue';
import { useFacilitiesStore } from '@/stores/data/facilities';
import {
  RoutineAssignmentSpecPrm,
  Facility,
  RoutineAssignmentPrm,
} from '@/api/__generated__/graphql';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import DeleteExecutionModal from '../../modal/DeleteExecutionModal.vue';
import { IconTrash01 } from '@/components/icons';

const props = defineProps<{
  facility: Facility;
}>();
const emit = defineEmits(['width', 'updateExecutions']);

const modalStore = useModalStore();
const facilitiesStore = useFacilitiesStore();
const facilityTypesStore = useFacilityTypesStore();
const routinesStore = useRoutinesStore();

const routines = computed(() => routinesStore.routines);

const routineExecutions = ref<RoutineAssignmentPrm[]>([]);

// Sorted Routine Executions
const sortedRoutineExecutions = computed(() => {
  return [...routineExecutions.value].sort((a, b) => (a.name || '').localeCompare(b.name || ''));
});

onMounted(() => {
  routineExecutions.value = (props.facility.routineAssignments || [])
    .filter((item) => item !== null)
    .map((item) => ({
      id: item.id,
      name: item.name,
      routineId: item.routineId,
      specs: (item.specs || []).filter((spec) => spec !== null),
    }));
});

//  Add Execution Modal Drawer
const addExecutionModalComRef = ref<InstanceType<typeof AddExecutionModal>>();
function handleClickAddExcution() {
  addExecutionModalComRef.value?.setModalOpen(true);
  if (!modalStore.isFacilitiesModalExpended) {
    emit('width', 'full');
    modalStore.isFacilitiesModalExpended = true;
    modalStore.isFacilitiesModalAutoExpended = true;
  }
}
function addNewExection(exec: RoutineAssignmentPrm) {
  routineExecutions.value.push(exec);
}

function getRoutineNameById(id: string) {
  const routine = routines.value.find((r) => r.id === id);
  return routine ? routine.name : '';
}

const facilityTypes = computed(() => facilityTypesStore.facilityTypes);
const roomProperties = computed(() => {
  const facilityType = facilityTypes.value.find(
    (facility) => facility.id === facilitiesStore.selectedFacility.typeId
  );
  return facilityType ? facilityType.properties : [];
});

function getPropertyNamesFromSpecs(specs: (RoutineAssignmentSpecPrm | null)[] | null) {
  if (!specs) return [];
  return specs
    .filter((spec) => spec !== null) // Filter out null specs
    .map((spec) => {
      const property = roomProperties.value?.find(
        (roomProperty) => roomProperty?.id === spec.propertyId
      );
      const option = property?.options?.find((opt) => opt?.id === spec.propertyValue);
      return {
        propertyName: property?.name || 'Unknown Property',
        optionName: option?.text || 'Unknown Option',
      };
    });
}

//  Edit Execution Modal Drawer

const activeExecutionIndex = ref<number>();
const activeExecution = ref<RoutineAssignmentPrm>({
  id: null,
  name: '',
  routineId: null,
  specs: [],
});
const editExecutionModalComRef = ref<InstanceType<typeof EditExecutionModal>>();

function handleClickEditExecution(exec: RoutineAssignmentPrm, index: number) {
  activeExecution.value = exec;
  activeExecutionIndex.value = index;
  editExecutionModalComRef.value?.setModalOpen(true);
  if (!modalStore.isFacilitiesModalExpended) {
    emit('width', 'full');
    modalStore.isFacilitiesModalExpended = true;
    modalStore.isFacilitiesModalAutoExpended = true;
  }
}
function updateExection(updateExec: RoutineAssignmentPrm) {
  const index = routineExecutions.value.findIndex((exec) => exec.id === updateExec.id);
  if (index !== -1) {
    routineExecutions.value[index] = { ...routineExecutions.value[index], ...updateExec };
  } else {
    // console.warn('Execution not found for updating:', updateExec);
  }
  // console.log('up exii', routineExecutions.value)
}

const deleteExecutionModalComRef = ref<InstanceType<typeof DeleteExecutionModal>>();

function handleClickDeleteExecution(exec: RoutineAssignmentPrm, index: number) {
  activeExecution.value = exec;
  activeExecutionIndex.value = index;
  deleteExecutionModalComRef.value?.setModalOpen(true);
}

function deleteExection(executionToDelete: RoutineAssignmentPrm) {
  routineExecutions.value = routineExecutions.value.filter(
    (exec) => exec.id !== executionToDelete.id
  );
}

function executionModalClosed() {
  if (modalStore.isFacilitiesModalAutoExpended) {
    emit('width', '5xl');
    modalStore.isFacilitiesModalExpended = false;
    modalStore.isFacilitiesModalAutoExpended = false;
  }
}
watch(
  routineExecutions,
  () => {
    const cleanedExecutions = routineExecutions.value.map((exec) => ({
      ...exec,
      specs:
        exec.specs
          ?.filter((spec): spec is { propertyId: any; propertyValue: any } => spec !== null) // Filter out null specs
          .map(({ propertyId, propertyValue }) => ({
            propertyId,
            propertyValue,
          })) || [],
    }));

    emit('updateExecutions', cleanedExecutions);
  },
  { deep: true }
);
</script>

<template>
  <!-- Routines -->
  <div>
    <div>
      <div class="font-semibold text-lg">Routines</div>
      <div class="font-medium text-sm pt-1">
        A Routine Assignment links a specific routine to a facility, ensuring the appropriate tasks
        are scheduled and tracked consistently. This helps keep the facility compliant and
        organized.
      </div>
    </div>

    <!--  Add Execution Button -->
    <div class="flex justify-end mt-4 py-[14px]">
      <fwb-button
        @click="handleClickAddExcution"
        class="bg-primary-600 hover:bg-brand-600 font-semibold inline-flex"
        pill
        :disabled="facility?.archived ?? false"
      >
        Add Assignment
      </fwb-button>
    </div>

    <!-- Table -->
    <div class="overflow-x-auto">
      <base-table>
        <t-head class="uppercase">
          <t-r>
            <t-h class="min-w-48 text-nowrap"> Name </t-h>
            <t-h class="min-w-28 text-nowrap">routine</t-h>
            <t-h class="min-w-48 text-nowrap">Assigned to</t-h>
            <t-h class="min-w-10 text-nowrap"></t-h>
          </t-r>
        </t-head>
        <t-body v-if="sortedRoutineExecutions.length > 0">
          <t-r
            v-for="(exec, index) in sortedRoutineExecutions"
            :key="index"
            class="hover:bg-slate-50"
          >
            <t-d class="font-semibold text-brand-600">
              <a @click.prevent="handleClickEditExecution(exec, index)" href="#">
                {{ exec.name }}
              </a>
            </t-d>
            <t-d>
              {{ getRoutineNameById(exec.routineId) }}
            </t-d>
            <t-d class="max-w-[120px]">
              <template v-if="getPropertyNamesFromSpecs(exec.specs).length > 0">
                <fwb-tooltip placement="bottom" class="!block">
                  <template #trigger>
                    <div class="truncate ...">
                      <span
                        v-for="(assigned, index) in getPropertyNamesFromSpecs(exec.specs)"
                        :key="index"
                      >
                        {{ assigned.propertyName }} : {{ assigned.optionName }}
                        {{ getPropertyNamesFromSpecs(exec.specs).length === index + 1 ? '' : ',' }}
                        &nbsp;
                      </span>
                    </div>
                  </template>
                  <template #content>
                    <span
                      v-for="(assigned, index) in getPropertyNamesFromSpecs(exec.specs)"
                      :key="index"
                    >
                      {{ assigned.propertyName }} : {{ assigned.optionName }}
                      {{ getPropertyNamesFromSpecs(exec.specs).length === index + 1 ? '' : ',' }}
                      &nbsp;
                    </span>
                  </template>
                </fwb-tooltip>
              </template>
              <template v-else> Entire Facility </template>
            </t-d>
            <t-d>
              <a
                @click.prevent="handleClickDeleteExecution(exec, index)"
                href="#"
                :class="[facility?.archived ? 'pointer-events-none' : '']"
              >
                <IconTrash01 height="16" width="16" />
              </a>
            </t-d>
          </t-r>
        </t-body>
        <t-body v-else>
          <t-r>
            <t-d colspan="3" class="text-center">No Executions to Display</t-d>
          </t-r>
        </t-body>
      </base-table>
    </div>

    <!-- Add Execution Modal Drawer -->
    <AddExecutionModal
      ref="addExecutionModalComRef"
      :facility="props.facility"
      @close="executionModalClosed"
      @new-exec="addNewExection"
    />

    <!-- Edit Execution Modal Drawer -->
    <EditExecutionModal
      ref="editExecutionModalComRef"
      :exec="activeExecution"
      :facility="props.facility"
      @close="executionModalClosed"
      @updated-exec="updateExection"
    />

    <!-- Delete Execution Modal Drawer -->
    <DeleteExecutionModal
      ref="deleteExecutionModalComRef"
      :exec="activeExecution"
      @close="executionModalClosed"
      @deleted-exec="deleteExection"
    />
  </div>
</template>

<style lang="css" scoped></style>
