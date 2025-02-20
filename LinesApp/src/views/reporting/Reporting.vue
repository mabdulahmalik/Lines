<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch } from 'vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import PageContainer from '@/containers/PageContainer.vue';
import PageHeader from '@/components/header/PageHeader.vue';
import { FwbButton, FwbDropdown, FwbListGroup, FwbListGroupItem } from 'flowbite-vue';
import { IconChevronDown } from '@/components/icons/index';
import Modal from '@/components/modal/Modal.vue';
import CheckListItem from '@/components/list/CheckListItem.vue';

// Define available reports
const reports = [
  { name: 'Procedures', file: 'Procedures.trdp' },
  { name: 'Performance', file: 'Performance.trdp' },
  { name: 'Work Log', file: 'WorkLog.trdp' },
  { name: 'Response Time', file: 'ResponseTime.trdp' },
  // { name: 'Dashboard', file: 'Dashboard.trdp' }
];

const breadcrumbStore = useBreadcrumbStore();

// Set breadcrumbs for the reporting page
onMounted(() => {
  breadcrumbStore.breadcrumbs = [{ title: 'Reporting', to: '/' }, { title: reports[0].name }];
});

onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

// State to store the selected report
const selectedReport = ref(reports[0]);

// Function to initialize or refresh the Telerik Report Viewer
const initTelerikReportViewer = (reportFile: string) => {
  let report: any;

  const reportViewerElement = document.getElementById('reportViewer1');
  if (reportViewerElement) {
    ($(reportViewerElement) as any).telerik_ReportViewer({
      serviceUrl: `${import.meta.env.VITE_REPORT_SERVICE_URL}/api/reports/`,
      reportSource: {
        report: reportFile,
      },
      viewMode: 'INTERACTIVE',
      scaleMode: 'FIT_PAGE_WIDTH',
      scale: 1.0,
      sendEmail: { enabled: false },
      enableAccessibility: false,
      parametersAreaPosition: 'RIGHT',
      parametersAreaVisible: false,
      parameters: {
        singleSelect: 'COMBO_BOX',
      },
      pageReady: (event: any) => {
        const reportViewer = $('#reportViewer1').data('telerik_ReportViewer');
        reportViewer.parametersAreaVisible(false);
        reportViewer.scale({ scaleMode: 'FIT_PAGE_WIDTH', scale: 1.0 });

        if (report != event.currentTarget.reportSource().report) {
          // Adjust the height of the report viewer container
          const container = document.getElementsByClassName('trv-page-wrapper')[0];
          const newHeight = container.scrollHeight + 300;

          const reportViewerElement = document.getElementById('reportViewer1');
          if(reportViewerElement) reportViewerElement.style.height = `${newHeight}px`;

          reportViewer.refreshReport();
          $('#reportViewer1').parent().scrollTop(0);

          report = event.currentTarget.reportSource().report;
        }
      },
    });
  }
};

const changeReportSource = (newReport: any) => {
  const reportViewer = $('#reportViewer1').data('telerik_ReportViewer');
  if (reportViewer) {
    reportViewer.reportSource({
      report: newReport.file,
    });

    $('#reportViewer1').parent().scrollTop(0);
    breadcrumbStore.breadcrumbs = [{ title: 'Reporting', to: '/' }, { title: newReport.name }];
  }
};

// Watch for changes in the selected report and update the report viewer
watch(selectedReport, (newReport) => {
  changeReportSource(newReport);
});

// Initialize the Telerik Report Viewer on mount
onMounted(() => {
  initTelerikReportViewer(selectedReport.value.file);
});

// Method to handle dropdown item click
const selectReport = (report: any) => {
  selectedReport.value = report;
};

// Select report modal
const selectReportModal = ref<InstanceType<typeof Modal>>();
function openReportSeletModal() {
  selectReportModal.value?.setModalOpen(true);
}
</script>

<template>
  <div>
    <!-- Page Header for breadcrumbs -->
    <PageHeader>
      <div class="flex items-center justify-between gap-4 py-1 overflow-x-visible">
        <h1></h1>
        <!-- Desktop only select report -->
        <fwb-dropdown text="left" close-inside class="hidden lg:block">
          <template #trigger>
            <fwb-button class="bg-primary-600 hover:bg-brand-600" pill>
              {{ selectedReport.name }}
              <template #suffix>
                <IconChevronDown class="ml-2" />
              </template>
            </fwb-button>
          </template>
          <fwb-list-group>
            <fwb-list-group-item
              v-for="report in reports"
              :key="report.name"
              :value="report.file"
              @click="selectReport(report)"
              class="hover:bg-gray-100 cursor-pointer"
            >
              {{ report.name }}
            </fwb-list-group-item>
          </fwb-list-group>
        </fwb-dropdown>
        <!-- Mobile only select report-->
        <fwb-button
          @click="openReportSeletModal"
          class="lg:hidden bg-primary-600 hover:bg-brand-600"
          pill
        >
          {{ selectedReport.name }}
          <template #suffix>
            <IconChevronDown class="ml-2" />
          </template>
        </fwb-button>
      </div>
    </PageHeader>

    <!-- Page Container for the main content -->
    <PageContainer>
      <div id="reportViewer1">Loading...</div>
    </PageContainer>

    <!-- Select report modal for mobile devices -->
    <Modal ref="selectReportModal" title="Group by" no_footer :z_index="55" :close_outside="false">
      <template #body>
        <div class="flex flex-col gap-2 p-4">
          <CheckListItem
            v-for="report in reports"
            :key="report.name"
            :value="report.file"
            :checked="selectedReport.file === report.file"
            @click="selectReport(report)"
          >
            {{ report.name }}
          </CheckListItem>
        </div>
      </template>
    </Modal>
  </div>
</template>

<style scoped>
#reportViewer1 {
  position: relative;
  width: 100%;
  height: 1300px;
}

.fwb-list-group-item {
  padding: 8px 12px;
}

.fwb-list-group-item:hover {
  background-color: #f3f4f6;
  cursor: pointer;
}
</style>
