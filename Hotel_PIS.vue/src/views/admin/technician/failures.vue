<template>
    <newFailure/>
    <el-row v-if="failures">
        <el-col :span="24">
            <failure v-for="failure in failures" :key="failure.id" :failure="failure" />
        </el-col>
    </el-row>
</template>
<script lang="js">
import failure from "@/components/admin/technician/failure.vue";
import newFailure from "@/components/admin/technician/newFailure.vue";

export default {
  components: {
    failure,
    newFailure
  },
  data() {
    return {
      failures: null,
    };
  },
  created() {
    this.loadFailures();
  },
  methods: {
    loadFailures() {
      this.failures = null;
      //this.$root.loading = !this.$root.loading
      fetch('api/Failure/GetAll')
          .then(r => r.json())
          .then(json => {
            this.failures = json;
          });
    }
  }
}
</script>
<style scoped>
    .failures {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
    }
</style>