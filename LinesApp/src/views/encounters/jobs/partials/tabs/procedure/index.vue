<script setup lang="ts">
import IconAlertTriangle from '@/components/icons/IconAlertTriangle.vue';
import IconMinusCircle from '@/components/icons/IconMinusCircle.vue';
import IconPlusCircle from '@/components/icons/IconPlusCircle.vue';
import { FwbButton, FwbBadge, FwbInput, FwbSelect, FwbToggle, FwbAlert } from 'flowbite-vue';
import { computed, onMounted, reactive, ref, watch } from 'vue';
// import { useModalStore } from '@/stores/modal';
// import AdjustFollowUpDateModal from './AdjustFollowUpDateModal.vue';
import ConformationModal from '@/components/modal/ConformationModal.vue';
import OutlineIcon from '@/components/styled-icon/OutlineIcon.vue';
import { IconAlertCircle, IconEdit03, IconEdit05 } from '@/components/icons';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import {
  ApplyProcedureToEncounterCmd,
  Encounter,
  EncounterProcedure,
  EncounterProcedureValue,
  EncounterStage,
  ModifyProcedureForEncounterCmd,
  ProcedureFieldSetting,
  ProcedureType,
} from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';
import ButtonCheckbox from '@/components/form/ButtonCheckbox.vue';
import ButtonRadio from '@/components/form/ButtonRadio.vue';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { useRoutinesStore } from '@/stores/data/settings/routines';
import { useFacilitiesStore } from '@/stores/data/facilities';
import moment from 'moment';

const props = defineProps<{
  encounter?: Encounter;
  isReview?: boolean;
  isEditable?: boolean;
}>();

const emit = defineEmits<{
  (e: 'has-procedures', val: boolean): void;
  (e: 'is-procedures-unsaved', val: boolean): void;
}>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

// const modalStore = useModalStore();
const proceduresStore = useProceduresStore();
const encountersStore = useEncountersStore();
const linesStore = useLinesStore();
const routinesStore = useRoutinesStore();
const facilitiesStore = useFacilitiesStore();

const procedureOptions = computed(() =>
  proceduresStore.procedures
    .filter((pro) => pro.archived === false)
    .map((p) => ({
      value: p.id,
      name: p.name ?? '',
    }))
);

const linesOptions = computed(() =>
  linesStore.lines
    .filter((line) => props.encounter?.lines?.includes(line.id))
    .map((l) => ({
      value: l.id,
      name: l.name ?? '',
    }))
);

const encounterProcedures = ref<(EncounterProcedure & { isSaved?: boolean })[]>([]);
const procedureWithLines = ref<
  { id: string; lineName: string; procedureId: string; confirmLine: boolean; removalLineId: '' }[]
>([]);

function getExistingLineName(procedureId: string) {
  const line = linesStore.lines.find(
    (l) =>
      l.insertedWith?.encounterId === props.encounter?.id &&
      l.insertedWith?.encounterProcedureId === procedureId
  );
  return line?.name ?? '';
}

function getLineNameById(lineId: string) {
  return linesStore.lines.find((l) => l.id === lineId)?.name ?? '';
}
watch(
  () => props.encounter?.procedures,
  (newProcedures) => {
    const draft = encounterProcedures.value?.filter((p) => p?.isSaved === false);
    encounterProcedures.value = (
      newProcedures?.filter((procedure): procedure is EncounterProcedure => procedure !== null) ||
      []
    )
      .concat(draft)
      .map((procedure) => {
        // Find the corresponding procedure in proceduresStore
        const selectedProcedure = proceduresStore.procedures.find(
          (p) => p.id === procedure.procedureId
        );

        // Map current procedure values without __typename and add missing fields from selectedProcedure
        const updatedValues = [
          // Existing values with __typename removed
          ...(procedure.values?.map(({ __typename, ...rest }: any) => rest) || []),

          // Add missing fields from selectedProcedure if not already in values
          ...(selectedProcedure?.fields
            ?.filter((field) => !procedure.values?.some((value) => value?.fieldId === field?.id))
            .map((field) => ({
              fieldId: field?.id,
              value: field?.type === 'TOGGLE' ? 'off' : '',
            })) || []),
        ];

        return {
          ...procedure,
          values: updatedValues,
        };
      });
    // For Lines
    procedureWithLines.value = (
      newProcedures?.filter((procedure): procedure is EncounterProcedure => procedure !== null) ||
      []
    )
      .concat(draft)
      .map((procedure) => {
        return {
          id: procedure.id,
          procedureId: procedure.procedureId,
          lineName: getExistingLineName(procedure.id),
          confirmLine: false,
          removalLineId: '',
        };
      });
  },
  { immediate: true } // Trigger the watcher immediately on component mount
);

watch(
  () => linesStore.lines,
  (newLines) => {
    procedureWithLines.value = (
      props.encounter?.procedures?.filter(
        (procedure): procedure is EncounterProcedure => procedure !== null
      ) || []
    ).map((procedure) => {
      return {
        id: procedure.id,
        procedureId: procedure.procedureId,
        lineName: getExistingLineName(procedure.id),
        confirmLine: false,
        removalLineId: '',
      };
    });
    console.log('Lines Update', newLines);
  }
);

function handleClickAddProcedure() {
  encounterProcedures.value.push({
    id: '',
    performAt: '',
    performedBy: '',
    procedureId: '',
    values: [],
    isSaved: false,
  });
  procedureWithLines.value.push({
    id: '',
    lineName: '',
    procedureId: '',
    confirmLine: false,
    removalLineId: '',
  });
}

function changeSelectedProcedure(encProc: EncounterProcedure, index: number) {
  const selectedProcedure =
    proceduresStore.procedures.find((p) => p.id === encProc.procedureId) || null;
  encounterProcedures.value[index] = {
    ...encProc,
    id: encProc.id,
    performAt: encProc.performAt,
    performedBy: encProc.performedBy,
    procedureId: encProc.procedureId,
    values:
      selectedProcedure?.fields?.map((field) => ({
        fieldId: field?.id,
        value: field?.type === 'TOGGLE' ? 'off' : '',
      })) || [],
  };
  if (selectedProcedure?.type === ProcedureType.Insertion) {
    procedureWithLines.value[index] = {
      id: encProc.id,
      lineName: '',
      procedureId: encProc.procedureId,
      confirmLine: false,
      removalLineId: '',
    };
  } else if (selectedProcedure?.type === ProcedureType.Removal) {
    procedureWithLines.value[index] = {
      id: encProc.id,
      lineName: '',
      procedureId: encProc.procedureId,
      confirmLine: false,
      removalLineId: props.encounter?.lines?.[0] ?? '',
    };
  }
}

function isRemovalProcedure(encProc: EncounterProcedure) {
  const selectedProcedure =
    proceduresStore.procedures.find((p) => p.id === encProc.procedureId) || null;
  return selectedProcedure?.type === ProcedureType.Removal || false;
}

function isInsertionProcedure(encProc: EncounterProcedure) {
  const selectedProcedure =
    proceduresStore.procedures.find((p) => p.id === encProc.procedureId) || null;
  return selectedProcedure?.type === ProcedureType.Insertion || false;
}

function getFieldNameById(fieldId: string): string {
  return (
    proceduresStore.procedures
      .map((procedure) => procedure.fields?.find((f) => f?.id === fieldId))
      .find((field) => field)?.name || ''
  );
}

const getInputType = (encProcId: string, fieldId: string) => {
  const selectedProcedure = proceduresStore.procedures.find((p) => p.id === encProcId);
  if (!selectedProcedure) return null;

  const selectedField = selectedProcedure.fields?.find((f) => f?.id === fieldId);

  if (selectedField?.type === 'LIST') {
    if (selectedField.settings?.includes(ProcedureFieldSetting.MultiSelect)) {
      return 'multi-select';
    } else {
      return 'select';
    }
  } else if (selectedField?.type === 'TEXT') {
    return 'text';
  } else if (selectedField?.type === 'NUMBER') {
    return 'number';
  } else if (selectedField?.type === 'TOGGLE') {
    return 'toggle';
  }

  return null;
};

const getField = (encProcId: string, fieldId: string) => {
  const selectedProcedure = proceduresStore.procedures.find((p) => p.id === encProcId);
  if (!selectedProcedure) return null;

  const selectedField = selectedProcedure.fields?.find((f) => f?.id === fieldId);

  return selectedField;
};

// Method to get the toggle value
function getToggleValue(val: string): boolean {
  return val === 'on';
}
// Method to update the toggle value
function updateToggleValue(proInd: number, fieldInd: number, value: boolean) {
  const procedure = encounterProcedures.value?.[proInd];
  const procedureValues = procedure?.values?.[fieldInd];
  if (procedure && procedureValues) {
    procedureValues.value = value ? 'on' : 'off';
  }
}

function getMultiSelectValue(val: string): string[] {
  if (!val) return [];
  // Split the comma-separated IDs and find corresponding option objects
  const selectedIds = val.split(',');
  return selectedIds;
}

function updateMultiSelectValue(proInd: number, fieldInd: number, newValue: string[]) {
  const procedure = encounterProcedures.value?.[proInd];
  const procedureValues = procedure?.values?.[fieldInd];
  if (procedure && procedureValues) {
    procedureValues.value = newValue.join(',');
  }
}

function getProcedureNameById(procedureId: string): string {
  return proceduresStore.procedures.find((p) => p.id === procedureId)?.name || '';
}

function getSelectedOptionNameById(optionId: string): string {
  for (const procedure of proceduresStore.procedures) {
    for (const field of procedure.fields || []) {
      const option = field?.options?.find((opt) => opt?.id === optionId);
      if (option) {
        return option.text;
      }
    }
  }
  return '';
}

// Remove procedure
const conformationModalRef = ref<InstanceType<typeof ConformationModal>>();
const deleteEncProcedureId = ref<string>('');
const deleteProcedureId = ref<string>('');

function handleClickRemoveProcedure(index: number, encProcId: string, procId: string) {
  if (encProcId === '') {
    encounterProcedures.value.splice(index, 1);
    procedureWithLines.value.splice(index, 1);
    return;
  }
  conformationModalRef.value?.setModalOpen(true);
  deleteEncProcedureId.value = encProcId;
  deleteProcedureId.value = procId;
}
function handleCancelDelete() {
  conformationModalRef.value?.setModalOpen(false);
}

const proceduresToBeDeleted = ref<string[]>([]);

function handleConfirmDelete() {
  if (deleteEncProcedureId.value && props.encounter) {
    proceduresToBeDeleted.value.push(deleteEncProcedureId.value)
    conformationModalRef.value?.setModalOpen(false);
    deleteEncProcedureId.value = '';
    deleteProcedureId.value = '';
    console.log(proceduresToBeDeleted.value)
  }
}

const isEncProcedureRemoved = (id: string) => {
  return proceduresToBeDeleted.value.includes(id);
};
function getIndexPlusOne(oldIndex: number){
  return encounterProcedures.value.slice(0, oldIndex).filter(p => !isEncProcedureRemoved(p.id)).length + 1
}

function restoreProcedure(id: string) {
  const index = proceduresToBeDeleted.value.indexOf(id);
  if (index > -1) {
    proceduresToBeDeleted.value.splice(index, 1);
  }
}
const errors = reactive<{ [key: number]: { [key: string]: string } }>({});
function clearFieldError(index: number, fieldId: string) {
  if (errors[index] && errors[index][fieldId]) {
    delete errors[index][fieldId];
    if (Object.keys(errors[index]).length === 0) {
      delete errors[index];
    }
  }
}
const lineErrors = reactive<{ [key: number]: string }>({});
function clearLineError(index: number) {
  if (lineErrors[index]) {
    delete lineErrors[index];
  }
}
function validateFields() {
  encounterProcedures.value.forEach((procedure, index) => {
    if (!errors[index]) {
      errors[index] = {};
    }
    procedure.values?.forEach((field) => {
      const selectedProcedure = proceduresStore.procedures.find((p) =>
        p.fields?.some((f) => f?.id === field?.fieldId)
      );
      const selectedField = selectedProcedure?.fields?.find((f) => f?.id === field?.fieldId);

      if (selectedField?.settings?.includes(ProcedureFieldSetting.Required) && !field?.value) {
        errors[index][field?.fieldId] = `${getFieldNameById(field?.fieldId)} is required`;
      } else {
        delete errors[index][field?.fieldId];
      }
    });
    if (Object.keys(errors[index]).length === 0) {
      delete errors[index];
    }
  });
}

function validateLines() {
  encounterProcedures.value.forEach((procedure, index) => {
    if (!lineErrors[index]) {
      lineErrors[index] = '';
    }
    const selectedProcedure =
      proceduresStore.procedures.find((p) => p.id === procedure.procedureId) || null;
    const lineName = procedureWithLines.value[index].lineName;
    if (
      procedure.id === '' &&
      selectedProcedure?.type === ProcedureType.Insertion &&
      lineName === ''
    ) {
      lineErrors[index] = 'Line name is required';
    } else {
      delete lineErrors[index];
    }
  });
}

function handleConfirmLine(index: number) {
  validateLines();
  if (lineErrors[index]) {
    return;
  }
  procedureWithLines.value[index].confirmLine = true;
}

const reviewEditIndex = ref<number | null>(null);

function isProcedureDirty(procedure: EncounterProcedure): boolean {
  const originalProcedure = props.encounter?.procedures?.find((p) => p?.id === procedure.id);
  if (!originalProcedure) return true;
  const currentValues = procedure.values?.filter((v) => v?.value !== '') || [];
  const originalValues =
    originalProcedure.values?.map(({ __typename, ...rest }: any) => rest) || [];
  return JSON.stringify(currentValues) !== JSON.stringify(originalValues);
}

async function saveProgress(): Promise<boolean> {
  validateFields();
  validateLines();
  if (Object.keys(errors).length > 0 || Object.keys(lineErrors).length > 0) {
    return false;
  }
  const encounterId = props.encounter?.id;

  for (const procedure of encounterProcedures.value) {
    const values = procedure.values?.filter((v) => v?.value !== '') || [];

    if (!procedure.id && procedure.procedureId) {
      // Get routine IDs
      const routineIds = routinesStore.routines
        .filter(
          (x) =>
            x.followUp &&
            x.origin?.some((o) => o?.procedureId == procedure.procedureId)
        )
        .map((x) => x.id);

      // Get routine assignment IDs
      const facility = facilitiesStore.facilities!.find(
        (x) => x.id == props.encounter?.location?.facilityId
      )!;
      const routineAssignmentIds = facility
        .routineAssignments!.filter((ra) => routineIds.includes(ra?.routineId))
        .map((x) => x!.id);

      // Create and apply a new procedure
      const newProcedure: ApplyProcedureToEncounterCmd = {
        encounterId: encounterId,
        procedureId: procedure.procedureId,
        values: values,
        insertedLineName: getInsertionLineName(procedure.procedureId),
        removedLineId: getRemovalLineId(procedure.procedureId),
        routinesAssigned: routineAssignmentIds,
        routinesRemoved: [],
      };

      await encountersStore.applyProcedureToEncounter(newProcedure);
      procedure.isSaved = true;
    } else if (procedure.id && isProcedureDirty(procedure)) {
      // Modify existing procedure
      const updatedProcedure: ModifyProcedureForEncounterCmd = {
        encounterId: encounterId,
        id: procedure.id,
        values: values,
      };

      await encountersStore.modifyProcedureForEncounter(updatedProcedure);
    }
  }

  if(proceduresToBeDeleted.value.length > 0){
    for (const encProcedureId of proceduresToBeDeleted.value){
      console.log(encProcedureId)
      await encountersStore.removeProcedureFromEncounter({
        encounterId: encounterId,
        id: encProcedureId,
      });
    }
  }
  return true;
}

function getInsertionLineName(procedureId: string) {
  const lineName = procedureWithLines.value.find((p) => p.procedureId === procedureId)?.lineName;
  return lineName || null;
}

function getRemovalLineId(procedureId: string) {
  if (props.encounter?.stage !== EncounterStage.Attending) return null;
  const removalLineId = procedureWithLines.value.find(
    (p) => p.procedureId === procedureId && !p.id
  )?.removalLineId;
  return removalLineId || null;
}

function getRoutineAssignment(encProc: EncounterProcedure) {
  const routineId = routinesStore.routines.find(
    (x) =>
      x.followUp &&
      x.origin?.some((o) => o?.procedureId == encProc.procedureId)
  )?.id;
  return facilitiesStore
    .facilities!.find((x) => x.id == props.encounter?.location?.facilityId)
    ?.routineAssignments?.find((ra) => ra?.routineId == routineId);
}

function getRoutineNextSchedule(encProc: EncounterProcedure) {
  const routine = routinesStore.routines.find(
    (x) =>
      x.followUp &&
      x.origin?.some((o) => o?.procedureId === encProc.procedureId)
  );

  if (!routine) return;

  let nextSchedule;
  let remainingDays;

  if (routine?.rolling) {
    // const unit = routine.rolling.interval?.unit;
    const value = routine.rolling.interval?.value;
    // Add the rolling interval to the current time
    nextSchedule = moment().add(value, 'day');
  } else if (routine?.recurrence) {
    const recurrence = routine.recurrence[0];
    const days = recurrence?.days;
    const time = recurrence?.time;

    if (!days) return;

    // Find the next occurrence of the specified day
    const today = moment();
    const currentDayOfWeek = today.day(); // Sunday = 0, Monday = 1, ..., Saturday = 6

    // Find the closest future day in the recurrence days
    const nextDayOfWeek = days?.find((day) => day > currentDayOfWeek) ?? days[0];
    const daysUntilNext =
      nextDayOfWeek > currentDayOfWeek
        ? nextDayOfWeek - currentDayOfWeek
        : 7 - (currentDayOfWeek - nextDayOfWeek);

    // Construct the next occurrence with the specified time
    nextSchedule = today
      .clone()
      .add(daysUntilNext, 'days')
      .set({
        hour: parseInt(time.split(':')[0], 10),
        minute: parseInt(time.split(':')[1], 10),
        second: parseInt(time.split(':')[2], 10),
      });
  }

  if (nextSchedule) {
    const now = moment();
    remainingDays = nextSchedule.diff(now, 'days');
  }

  //Tuesday, June 18th (7 Days)
  return nextSchedule?.format('dddd, MMMM Do') + ' ' + `(${remainingDays} Days)`;
}

onMounted(() => {
  if (encounterProcedures.value.length) {
    emit('has-procedures', true);
  } else {
    emit('has-procedures', false);
  }
});
watch(encounterProcedures.value, (newValue) => {
  if (newValue.filter((procedure) => procedure.procedureId !== '').length === 0) {
    emit('has-procedures', false);
  } else {
    emit('has-procedures', true);
  }
});

watch(
  () => props.encounter?.procedures,
  (newProcedures) => {
    if (newProcedures?.length === 0) {
      emit('has-procedures', false);
    } else {
      emit('has-procedures', true);
    }
  }
);

function isProcedureArchived(encProc: EncounterProcedure): boolean {
  return !!proceduresStore.procedures.find((p) => p.id === encProc.procedureId)?.archived;
}

function isFieldArchived(encProc: EncounterProcedure, fieldId: string): boolean {
  const selectedField = proceduresStore.procedures
    .find((p) => p.id === encProc.procedureId)
    ?.fields?.find((f) => f?.id === fieldId);
  return !!selectedField?.archived;
}

function isOptionArchived(encProc: EncounterProcedure, fieldId: string, optionId: string): boolean {
  const selectedOption = proceduresStore.procedures
    .find((p) => p.id === encProc.procedureId)
    ?.fields?.find((f) => f?.id === fieldId)
    ?.options?.find((o) => o?.id === optionId);
  return !!selectedOption?.archived;
}

function isFieldInUse(field: EncounterProcedureValue): boolean {
  if (field.value === '' || field.value === 'off') return false;
  return true;
}

function isOptionInUse(field: EncounterProcedureValue, optionId: string): boolean {
  if (field.value.split(',').includes(optionId)) return true;
  return false;
}

const isProceduresUnsaved = computed(() =>{
  if(proceduresToBeDeleted.value.length > 0){
    return true;
  }
  for (const procedure of encounterProcedures.value) {
    if (!procedure.id && procedure.procedureId) {
      return true
    } else if (procedure.id && isProcedureDirty(procedure)) {
      return true
    }
  }
  return false
})

watch(isProceduresUnsaved, (newValue) => {
  emit('is-procedures-unsaved', newValue)
});

defineExpose({
  saveProgress,
  encounterProcedures,
});
</script>

<template>
  <div class="max-w-3xl mx-auto flex flex-col gap-4">
    <!-- Procedures -->
    <div v-if="encounterProcedures.length" class="flex flex-col gap-5">
      <div
        v-for="(encProc, index) in encounterProcedures"
        class="flex pb-5 border-b border-slate-300"
        :key="index"
      >
        <div v-if="proceduresToBeDeleted.includes(encProc.id)" class="flex items-center justify-between  self-stretch w-full bg-slate-100 rounded-md px-4 py-2 ">
          <label class="text-sm font-semibold text-gray-900">{{
            getProcedureNameById(encProc.procedureId)
          }}</label>
          <div @click="restoreProcedure(encProc.id)" class="text-sm text-brand-700 font-medium cursor-pointer">
            Restore
          </div>
        </div>
        <div v-else class="flex items-start self-stretch w-full">
          <div class="flex flex-col h-full gap-2">
            <fwb-badge class="w-9 h-9">{{ getIndexPlusOne(index) }}</fwb-badge>
            <div class="w-[1px] bg-gray-200 flex-1 self-center -ml-1"></div>
          </div>
          <div class="-mt-1 flex-1">
            <!-- select and remove icon -->
            <div class="flex items-center">
              <div class="mr-2 flex-1 max-w-96">
                <div v-if="encProc.id" class="flex items-center min-h-10">
                  <label class="text-sm font-semibold text-gray-900">{{
                    getProcedureNameById(encProc.procedureId)
                  }}</label>
                </div>
                <fwb-select
                  v-else
                  v-model="encProc.procedureId"
                  :options="procedureOptions"
                  placeholder="Select Procedure"
                  @change="changeSelectedProcedure(encProc, index)"
                />
              </div>
              <fwb-button
                v-if="!props.isReview && props.isEditable"
                @click="handleClickRemoveProcedure(index, encProc.id, encProc.procedureId)"
                color="light"
                pill
                class="font-semibold px-0 hover:bg-white focus:ring-0 border-0 text-radical-red-500"
              >
                <template #prefix>
                  <IconMinusCircle />
                </template>
              </fwb-button>
              <fwb-button
                v-if="props.isReview && reviewEditIndex !== index && props.isEditable"
                color="light"
                pill
                square
                class="px-0 hover:bg-white focus:ring-0 border-0 ml-auto"
                @click="reviewEditIndex = index"
              >
                <IconEdit05 />
              </fwb-button>
            </div>
            <!-- Procedure -->
            <div
              v-if="encProc.values && encProc.values.length > 0"
              class="flex flex-col pt-4 lg:pt-6 gap-6 lg:gap-8 max-w-3xl"
              :class="{ 'bg-slate-50 rounded-lg p-4 lg:p-6 mt-4 lg:mt-6': props.isReview }"
            >
              <template v-for="(field, key) in encProc.values" :key="key">
                <template
                  v-if="field && (isFieldInUse(field) || !isFieldArchived(encProc, field.fieldId))"
                >
                  <!-- toggle/('on', 'off') -->
                  <div
                    v-if="getInputType(encProc.procedureId, field?.fieldId) === 'toggle'"
                    class="flex items-center gap-2"
                  >
                    <template v-if="props.isReview && reviewEditIndex !== index">
                      <label class="w-1/3 text-sm font-medium text-gray-500">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex-1">
                        <span class="font-semibold text-sm text-slate-800">{{
                          field.value === 'on' ? 'Yes' : 'No'
                        }}</span>
                      </div>
                    </template>
                    <fwb-toggle
                      v-else
                      v-bind:model-value="getToggleValue(field.value)"
                      @update:modelValue="updateToggleValue(index, key, $event)"
                      :label="getFieldNameById(field.fieldId)"
                      :disabled="
                        isProcedureArchived(encProc) || isFieldArchived(encProc, field.fieldId)
                      "
                    />
                  </div>
                  <!-- text/input -->
                  <div
                    v-if="
                      getInputType(encProc.procedureId, field.fieldId) === 'text' &&
                      (field.value || !props.isReview || reviewEditIndex === index)
                    "
                    class="flex items-center gap-2"
                  >
                    <template v-if="props.isReview && reviewEditIndex !== index">
                      <label class="w-1/3 text-xm font-medium text-gray-500">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex-1">
                        <span class="font-semibold text-sm text-slate-800">{{ field.value }}</span>
                      </div>
                    </template>
                    <template v-else>
                      <label class="w-1/3 text-sm font-medium text-gray-900">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex-1">
                        <fwb-input
                          v-model="field.value"
                          @input="clearFieldError(index, field.fieldId)"
                          type="text"
                          class="flex-1"
                          :class="errors[index]?.[field.fieldId] ? inputErrorClasses : ''"
                          :disabled="
                            isProcedureArchived(encProc) || isFieldArchived(encProc, field.fieldId)
                          "
                        />
                        <span
                          v-if="errors[index]?.[field.fieldId]"
                          class="flex flex-auto text-sm text-radical-red-600"
                        >
                          {{ errors[index][field.fieldId] }}
                        </span>
                      </div>
                    </template>
                  </div>
                  <!-- number/input -->
                  <div
                    v-if="
                      getInputType(encProc.procedureId, field.fieldId) === 'number' &&
                      (field.value || !props.isReview || reviewEditIndex === index)
                    "
                    class="flex items-center gap-2"
                  >
                    <template v-if="props.isReview && reviewEditIndex !== index">
                      <label class="w-1/3 text-xm font-medium text-gray-500">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex-1">
                        <span class="font-semibold text-sm text-slate-800">{{ field.value }}</span>
                      </div>
                    </template>
                    <template v-else>
                      <label class="w-1/3 text-sm font-medium text-gray-900">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex-1">
                        <fwb-input
                          :modelValue="field.value"
                          @update:modelValue="field.value = $event.toString()"
                          @input="clearFieldError(index, field.fieldId)"
                          type="number"
                          class="flex-1"
                          :class="errors[index]?.[field.fieldId] ? inputErrorClasses : ''"
                          :disabled="
                            isProcedureArchived(encProc) || isFieldArchived(encProc, field.fieldId)
                          "
                        />
                        <span
                          v-if="errors[index]?.[field.fieldId]"
                          class="flex flex-auto text-sm text-radical-red-600"
                        >
                          {{ errors[index][field.fieldId] }}
                        </span>
                      </div>
                    </template>
                  </div>
                  <!-- select/radio -->
                  <div
                    v-if="
                      getInputType(encProc.procedureId, field.fieldId) === 'select' &&
                      (field.value || !props.isReview || reviewEditIndex === index)
                    "
                    :class="{
                      'flex items-center gap-2': props.isReview && reviewEditIndex !== index,
                    }"
                  >
                    <template v-if="props.isReview && reviewEditIndex !== index">
                      <label class="w-1/3 text-xm font-medium text-gray-500">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex-1">
                        <span class="font-semibold text-sm text-slate-800">{{
                          getSelectedOptionNameById(field.value)
                        }}</span>
                      </div>
                    </template>
                    <template v-else>
                      <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex items-center flex-wrap gap-x-4">
                        <template
                          v-for="option in getField(encProc.procedureId, field.fieldId)?.options"
                        >
                          <ButtonRadio
                            v-if="
                              option &&
                              (isOptionInUse(field, option.id) ||
                                !isOptionArchived(encProc, field.fieldId, option.id))
                            "
                            v-model="field.value"
                            :label="option.text"
                            :value="option.id"
                            @input="clearFieldError(index, field.fieldId)"
                            class="my-2 w-full lg:w-[calc(50%-8px)]"
                            :disabled="
                              isProcedureArchived(encProc) ||
                              isFieldArchived(encProc, field.fieldId) ||
                              isOptionArchived(encProc, field.fieldId, option.id)
                            "
                          />
                        </template>
                      </div>
                      <span
                        v-if="errors[index]?.[field.fieldId]"
                        class="flex flex-auto text-sm text-radical-red-600"
                      >
                        {{ errors[index][field.fieldId] }}
                      </span>
                    </template>
                  </div>

                  <!-- multi-select/ checkbox -->
                  <div
                    v-if="
                      getInputType(encProc.procedureId, field.fieldId) === 'multi-select' &&
                      (field.value || !props.isReview || reviewEditIndex === index)
                    "
                    :class="{
                      'flex items-center gap-2': props.isReview && reviewEditIndex !== index,
                    }"
                  >
                    <template v-if="props.isReview && reviewEditIndex !== index">
                      <label class="w-1/3 text-xm font-medium text-gray-500">{{
                        getFieldNameById(field.fieldId)
                      }}</label>
                      <div class="flex-1">
                        <template
                          v-for="option in getField(encProc.procedureId, field.fieldId)?.options"
                        >
                          <span
                            v-if="
                              field.value
                                .split(',')
                                .map((item) => item.trim())
                                .includes(option?.id)
                            "
                            class="font-semibold text-sm text-slate-800"
                            >{{ option?.text }},&nbsp;
                          </span>
                        </template>
                      </div>
                    </template>
                    <template v-else>
                      <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white">{{
                        getFieldNameById(field?.fieldId)
                      }}</label>
                      <div class="flex items-center flex-wrap gap-x-4">
                        <template
                          v-for="option in getField(encProc.procedureId, field.fieldId)?.options"
                        >
                          <ButtonCheckbox
                            v-if="
                              option &&
                              (isOptionInUse(field, option.id) ||
                                !isOptionArchived(encProc, field.fieldId, option.id))
                            "
                            :modelValue="getMultiSelectValue(field.value)"
                            @update:modelValue="updateMultiSelectValue(index, key, $event)"
                            :label="option.text"
                            :value="option.id"
                            @input="clearFieldError(index, field.fieldId)"
                            class="my-2 w-full lg:w-[calc(50%-8px)]"
                            :disabled="
                              isProcedureArchived(encProc) ||
                              isFieldArchived(encProc, field.fieldId) ||
                              isOptionArchived(encProc, field.fieldId, option.id)
                            "
                          />
                        </template>
                      </div>
                      <span
                        v-if="errors[index]?.[field.fieldId]"
                        class="flex flex-auto text-sm text-radical-red-600"
                      >
                        {{ errors[index][field.fieldId] }}
                      </span>
                    </template>
                  </div>
                </template>
              </template>
              <!-- Save/update -->
            </div>
            <!-- New Line Alert -->
            <fwb-alert
              v-if="
                isInsertionProcedure(encProc) &&
                ((procedureWithLines[index].id && procedureWithLines[index].lineName) ||
                  procedureWithLines[index].id === '')
              "
              class="bg-white mt-5 lg:mt-8 !p-0 rounded-lg max-w-md"
            >
              <div class="flex items-center gap-2 bg-brand-600 text-white py-2 px-4 rounded-t-lg">
                <template
                  v-if="procedureWithLines[index].confirmLine || procedureWithLines[index].id"
                >
                  <span class="text-sm font-semibold">New Line created from Procedure</span>
                </template>

                <template v-else>
                  <IconAlertTriangle />
                  <span class="text-sm font-semibold">This Procedure will create a new Line</span>
                </template>
              </div>
              <div
                v-if="
                  procedureWithLines[index].confirmLine ||
                  (procedureWithLines[index].id && procedureWithLines[index].lineName)
                "
                class="flex flex-col bg-brand-100 text-slate-900 p-4 rounded-b-lg"
              >
                <div class="flex justify-between">
                  <label class="mb-1 block text-sm font-medium text-brand-600">{{
                    procedureWithLines[index].lineName
                  }}</label>
                  <fwb-button
                    @click="procedureWithLines[index].confirmLine = false"
                    color="light"
                    pill
                    square
                    class="bg-brand-100 border-brand-100 focus:ring-0"
                  >
                    <IconEdit03
                      v-if="!(procedureWithLines[index].id && procedureWithLines[index].lineName)"
                    />
                  </fwb-button>
                </div>
              </div>
              <div v-else class="flex flex-col bg-brand-100 text-slate-900 p-4 rounded-b-lg">
                <div>
                  <label class="mb-1 block text-sm font-medium text-gray-900 dark:text-white"
                    >Name for new line</label
                  >
                  <fwb-input
                    v-model="procedureWithLines[index].lineName"
                    @input="clearLineError(index)"
                  />
                  <span
                    v-if="lineErrors[index]"
                    class="flex flex-auto text-sm text-radical-red-600"
                  >
                    {{ lineErrors[index] }}
                  </span>
                </div>
                <div class="flex justify-end mt-2">
                  <fwb-button
                    outline
                    pill
                    @click="handleConfirmLine(index)"
                    class="border-brand-600 text-brand-600 hover:bg-brand-600"
                    >Confirm
                  </fwb-button>
                </div>
              </div>
            </fwb-alert>
            <!-- Removal Line Alert if multiple lines -->
            <fwb-alert
              v-if="
                isRemovalProcedure(encProc) &&
                !procedureWithLines[index].id &&
                props.encounter?.lines?.length
              "
              class="bg-white mt-5 lg:mt-8 !p-0 rounded-lg max-w-md"
            >
              <div class="flex items-center gap-2 bg-brand-600 text-white py-2 px-4 rounded-t-lg">
                <span class="text-sm font-bold">Line Removal</span>
              </div>
              <div class="bg-brand-50 text-slate-900 p-4 rounded-b-lg text-sm font-medium">
                <fwb-select
                  v-if="props.encounter?.lines?.length > 1"
                  v-model="procedureWithLines[index].removalLineId"
                  :options="linesOptions"
                  label="Select Line"
                />
                <div v-else class="flex items-center">
                  This Procedure will remove: &nbsp;
                  <label class="text-sm font-medium text-brand-600">{{
                    getLineNameById(procedureWithLines[index].removalLineId)
                  }}</label>
                </div>
              </div>
            </fwb-alert>
            <!-- Follow Up Alert -->
            <fwb-alert
              v-if="getRoutineAssignment(encProc)"
              class="bg-white mt-5 lg:mt-8 !p-0 rounded-lg"
            >
              <div
                class="flex items-center gap-2 bg-turquoise-blue-600 text-white py-2 px-4 rounded-t-lg"
              >
                <span class="text-sm font-bold">Follow up</span>
              </div>
              <div class="bg-cyan-50 text-slate-900 p-4 rounded-b-lg text-sm font-medium">
                <div>
                  {{ getRoutineAssignment(encProc)?.name }} on
                  <span class="text-turquoise-blue-600">
                    {{ getRoutineNextSchedule(encProc) }}</span
                  >
                </div>
              </div>
            </fwb-alert>
          </div>
        </div>
      </div>
    </div>

    <!-- Add procedure button -->
    <div v-if="!props.isReview && isEditable">
      <fwb-button
        @click="handleClickAddProcedure"
        color="light"
        pill
        class="font-semibold px-0 hover:bg-white focus:ring-0 border-0 text-brand-600"
      >
        <template #prefix>
          <IconPlusCircle />
        </template>
        Add procedure
      </fwb-button>
    </div>

    <!-- Conformation Modal -->
    <ConformationModal ref="conformationModalRef" :z_index="101" no_footer>
      <template #body>
        <div class="p-4 lg:p-6 !pt-0 flex flex-col gap-5 text-center">
          <div class="flex flex-col gap-1">
            <div class="flex justify-center">
              <OutlineIcon size="xl" type="error">
                <IconAlertCircle></IconAlertCircle>
              </OutlineIcon>
            </div>
            <div class="text-base font-medium text-slate-900">
              Are you sure you want to remove procedure “{{
                getProcedureNameById(deleteProcedureId)
              }}”?
            </div>
          </div>
          <div class="flex justify-center items-center gap-4 w-full">
            <fwb-button @click="handleCancelDelete" color="light" pill> No, leave it</fwb-button>
            <fwb-button
              @click="handleConfirmDelete"
              color="red"
              class="bg-radical-red-600 hover:bg-radical-red-600"
              pill
            >
              Yes, I’m sure
            </fwb-button>
          </div>
        </div>
      </template>
    </ConformationModal>
  </div>
</template>
