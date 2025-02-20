<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';
import { Dropdown, DropdownMenu, DropdownItem } from '@/components/dropdown/index';
import {
  IconDotHorizontal,
  IconAlertHexagon,
  IconArrowCircleBrokenDownRight,
  IconXClose,
} from '@/components/icons';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { Line } from '@/api/__generated__/graphql';
import MarkHasInfectionModal from './modal/MarkHasInfectionModal.vue';
import { ref } from 'vue';
import ExternalPlacementModal from './modal/ExternalPlacementModal.vue';

const props = defineProps<{
  line: Line;
}>();

const linesStore = useLinesStore();

const markInfectionModalComRef = ref<InstanceType<typeof MarkHasInfectionModal>>();
const markExternalPlacementenModalComRef = ref<InstanceType<typeof ExternalPlacementModal>>();

function handleClickMarkInfection() {
  markInfectionModalComRef.value?.setModalOpen(true);
}

function handleClickMarkExternalPlacement() {
  markExternalPlacementenModalComRef.value?.setModalOpen(true);
}
</script>
<template>
  <div class="flex items-center">
    <Dropdown align-to-end class="rounded-lg" close-inside>
      <template #trigger>
        <fwb-button color="light" pill square>
          <IconDotHorizontal />
        </fwb-button>
      </template>
      <DropdownMenu class="min-w-64">
        <!-- Need to change static date to user selected date -->
        <DropdownItem v-if="!props.line.infectedOn" @click="handleClickMarkInfection">
          <IconAlertHexagon width="18" height="18" class="mr-2" />
          Mark as 'Has Infection'
        </DropdownItem>
        <DropdownItem v-else @click="linesStore.clearLineInfection({ id: props.line.id })">
          <IconXClose width="18" height="18" class="mr-2" />
          Remove 'Has Infection'
        </DropdownItem>
        <DropdownItem v-if="!props.line.externallyPlaced" @click="handleClickMarkExternalPlacement">
          <IconArrowCircleBrokenDownRight width="18" height="18" class="mr-2" />
          Mark as 'External Placement'
        </DropdownItem>
        <DropdownItem v-else @click="linesStore.placeLineInternally({ id: props.line.id })">
          <IconXClose width="18" height="18" class="mr-2" />
          Remove 'External Placement'
        </DropdownItem>
      </DropdownMenu>
    </Dropdown>
    <MarkHasInfectionModal ref="markInfectionModalComRef" :line="props.line" />
    <ExternalPlacementModal ref="markExternalPlacementenModalComRef" :line="props.line" />
  </div>
</template>
